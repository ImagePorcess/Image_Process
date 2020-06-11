# coding=utf-8
import operator
from math import log
import matplotlib.pyplot as plt


def createDataSet():
    dataSet = [['晴天', '热','高','无', 'no'],
               ['晴天', '热', '高', '有', 'no'],
               ['阴天', '热', '高', '无', 'yes'],
               ['雨天', '温', '高', '无', 'yes'],
               ['雨天', '冷', '正常', '无', 'yes'],
               ['雨天', '冷', '正常', '有', 'no'],
               ['阴天', '冷', '正常', '有', 'yes'],
               ['晴天', '温', '高', '无', 'no'],
               ['晴天', '冷', '正常', '无', 'yes'],
               ['雨天', '温', '正常', '无', 'yes'],
               ['晴天', '温', '正常', '有', 'yes'],
               ['阴天', '温', '高', '有', 'yes'],
               ['阴天', '热', '正常', '无', 'yes'],
               ['雨天', '温', '高', '有', 'no']]
    labels = ['天气', '温度','湿度','有风']
    return dataSet, labels


# 计算香农熵
def calcShannonEnt(dataSet):
    numEntries = len(dataSet)
    labelCounts = {}
    for feaVec in dataSet:
        currentLabel = feaVec[-1]
        if currentLabel not in labelCounts:
            labelCounts[currentLabel] = 0
        labelCounts[currentLabel] += 1
    shannonEnt = 0.0
    for key in labelCounts:
        prob = float(labelCounts[key]) / numEntries
        shannonEnt -= prob * log(prob, 2)
    return shannonEnt


def splitDataSet(dataSet, axis, value):
    retDataSet = []
    for featVec in dataSet:
        if featVec[axis] == value:
            reducedFeatVec = featVec[:axis]
            reducedFeatVec.extend(featVec[axis + 1:])
            retDataSet.append(reducedFeatVec)
    return retDataSet


def chooseBestFeatureToSplit(dataSet):
    numFeatures = len(dataSet[0]) - 1  # 因为数据集的最后一项是标签
    baseEntropy = calcShannonEnt(dataSet)
    bestInfoGain = 0.0
    bestFeature = -1
    for i in range(numFeatures):
        featList = [example[i] for example in dataSet]
        uniqueVals = set(featList)
        newEntropy = 0.0
        for value in uniqueVals:
            subDataSet = splitDataSet(dataSet, i, value)
            prob = len(subDataSet) / float(len(dataSet))
            newEntropy += prob * calcShannonEnt(subDataSet)
        infoGain = baseEntropy - newEntropy
        if infoGain > bestInfoGain:
            bestInfoGain = infoGain
            bestFeature = i
    return bestFeature


# 因为我们递归构建决策树是根据属性的消耗进行计算的，所以可能会存在最后属性用完了，但是分类
# 还是没有算完，这时候就会采用多数表决的方式计算节点分类
def majorityCnt(classList):
    classCount = {}
    for vote in classList:
        if vote not in classCount.keys():
            classCount[vote] = 0
        classCount[vote] += 1
    return max(classCount)


def createTree(dataSet, labels):
    classList = [example[-1] for example in dataSet]     #获取最后一列
    if classList.count(classList[0]) == len(classList):  # 类别相同则停止划分（如果只有一列类别数据）
        return classList[0]                                 #返回唯一类别数据
    if len(dataSet[0]) == 1:                            # 所有特征已经用完（数据只有一列）
        return majorityCnt(classList)                   #引入投票机制，选择最多的
    bestFeat = chooseBestFeatureToSplit(dataSet)            #选择最优列，得到最优列序号
    bestFeatLabel = labels[bestFeat]                    #活的最优列所有类别值
    myTree = {bestFeatLabel: {}}                        #初始化决策树
    del (labels[bestFeat])
    featValues = [example[bestFeat] for example in dataSet]
    uniqueVals = set(featValues)
    for value in uniqueVals:
        subLabels = labels[:]  # 为了不改变原始列表的内容复制了一下
        myTree[bestFeatLabel][value] = createTree(splitDataSet(dataSet,bestFeat, value), subLabels)
    return myTree
decisionNode = dict(boxstyle = "sawtooth", fc="0.8")
leafNode = dict(boxstyle = "round4" ,fc="0.8")
arrow_args = dict(arrowstyle="<-")

def plotNode(nodeTxt,centerPt,parentPt,nodeType):
    createPlot.ax1.annotate(nodeTxt,xy=parentPt,xycoords='axes fraction' ,\
                           xytext=centerPt ,textcoords='axes fraction',va="center" ,\
                            ha ="center" ,bbox=nodeType,arrowprops = arrow_args)

def getNumLeafs(myTree):
    numLeafs= 0
    firstStr =list(myTree.keys())[0]
    secondDict = myTree[firstStr]
    for key in secondDict.keys():
        if type(secondDict[key]).__name__=='dict':
            numLeafs +=getNumLeafs(secondDict[key])
        else :  numLeafs+=1
    return numLeafs

def getTreeDepth(myTree) :
    maxDepth=0
    firstStr =list(myTree.keys())[0]
    secondDict = myTree[firstStr]
    for key in secondDict.keys():
        if type(secondDict[key]).__name__=='dict':
            thisDepth = 1+ getTreeDepth(secondDict[key])
        else : thisDepth = 1
        if thisDepth > maxDepth : maxDepth=thisDepth
    return maxDepth

def plotMidText(cntrPt , parentPt ,txtString) :
    xMid = (parentPt[0]-cntrPt[0])/2.0 +cntrPt[0]
    yMid = (parentPt[1]-cntrPt[1])/2.0 +cntrPt[1]
    createPlot.ax1.text(xMid,yMid,txtString)

def plotTree(myTree, parentPt, nodeTxt):
    numLeafs = getNumLeafs(myTree)
    depth = getTreeDepth(myTree)
    firstStr = list(myTree.keys())[0]
    cntrPt = (plotTree.xOff + (1.0 + float(numLeafs)) / 2.0 / plotTree.totalW, plotTree.yOff)
    plotMidText(cntrPt, parentPt, nodeTxt)
    plotNode(firstStr, cntrPt, parentPt, decisionNode)
    secondDict = myTree[firstStr]
    plotTree.yOff = plotTree.yOff - 1.0 / plotTree.totalD
    for key in secondDict.keys():
        if type(secondDict[key]).__name__ == 'dict':
            plotTree(secondDict[key], cntrPt, str(key))
        else:
            plotTree.xOff = plotTree.xOff + 1.0 / plotTree.totalW
            plotNode(secondDict[key], (plotTree.xOff, plotTree.yOff), cntrPt, leafNode)
            plotMidText((plotTree.xOff, plotTree.yOff), cntrPt, str(key))
    plotTree.yOff = plotTree.yOff + 1.0 / plotTree.totalD

#分类算法代码
def classify(inputTree, featLabels, testVec):
    firstStr = list(inputTree.keys())[0]
    secondDict = inputTree[firstStr]
    featIndex = featLabels.index(firstStr)
    key = testVec[featIndex]
    valueOffset = secondDict[key]
    print ('+++', firstStr, 'xxx', secondDict, '---', key, '>>>', valueOffset)
    if isinstance(valueOffset, dict):
        classLabel = classify(valueOffset, featLabels, testVec)
    else:
        classLabel = valueOffset
    return classLabel

#创建绘图
def createPlot(inTree) :
    plt.rcParams['font.sans-serif'] = ['SimHei']  # 用来正常显示中文标签
    plt.rcParams['axes.unicode_minus'] = False  # 用来正常显示负号
    fig = plt.figure(1,facecolor = 'white')
    fig.clf()
    axprops =dict(xticks=[],yticks=[])
    createPlot.ax1 = plt.subplot(111,frameon = False ,**axprops)
    plotTree.totalW =float(getNumLeafs(inTree))
    plotTree.totalD=float(getTreeDepth(inTree))
    plotTree.xOff = -0.5/plotTree.totalW;plotTree.yOff = 1.0
    plotTree(inTree,(0.5,1.0),'')
    plt.show()

data, label = createDataSet()
myTree = createTree(data, label)
mydata, mylabel = createDataSet()
print(classify(myTree, mylabel, ['晴天', '热','高','无']))
print(classify(myTree, mylabel, ['阴天', '热', '高', '无']))
print(classify(myTree, mylabel, ['雨天', '温', '高', '无']))
createPlot(myTree)


def main():
    data, label = createDataSet()
    myTree = createTree(data, label)
    print(myTree)


if __name__ == '__main__':
    main()