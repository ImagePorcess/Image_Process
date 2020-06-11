import operator
from math import log
import matplotlib.pyplot as plt


def createDataSet():
    dataSet = [[ '杂食动物', '是', '否', '否', '是'],
               [ '杂食动物', '是', '否', '否', '是'],
               [ '肉食动物', '是', '否', '否', '是'],
               [ '肉食动物', '否', '否', '是', '否'],
               [ '肉食动物', '否', '是', '否', '否'],
               [ '肉食动物', '否', '否', '否', '否'],
               [ '杂食动物', '是', '否', '是', '是'],
               [ '草食动物', '是', '否', '否', '是'],
               [ '杂食动物', '否', '否', '是', '否'],
               [ '肉食动物', '否', '是', '否', '否'],
               [ '肉食动物', '是', '是', '否', '是'],
               [ '肉食动物', '否', '否', '否', '是'],
               [ '草食动物', '是', '否', '否', '是'],
               [ '肉食动物', '否', '否', '否', '否']]
    labels = ['饮食习惯', '胎生动物', '水生动物', '会飞']
    return dataSet, labels


#使用Python计算信息熵
def calcShannonEnt(dataSet):    #定义语法
    numEntries = len(dataSet)   #获取训练数据条数
    labelCounts = {}    #计算分类标签出现次数
    for feaVec in dataSet:  #迭代每条训练数据
        currentLabel = feaVec[-1]   #获取训练数据的标签值
        if currentLabel not in labelCounts: #标签值没有出现过，则新增标签
            labelCounts[currentLabel] = 0 #新增标签的计数值为0
        labelCounts[currentLabel] += 1 #对应标签的计数器加1
    shannonEnt = 0.0 #信息熵计数器
    for key in labelCounts: #迭代标签计数器集合
        prob = float(labelCounts[key]) / numEntries #求出标签出现概率
        shannonEnt -= prob * log(prob, 2) #累减信息熵
    return shannonEnt #返回信息熵


def splitDataSet(dataSet, axis, value):     #定义语法
    retDataSet = []                         #创建返回数据集
    for featVec in dataSet:                #遍历每行数据
        if featVec[axis] == value:          #处理指定特征值的数据行
            reducedFeatVec = featVec[:axis] #获取特征列前的数据行
            reducedFeatVec.extend(featVec[axis + 1:])   #获取特征列后的数据，并扩展到本行
            retDataSet.append(reducedFeatVec)   #追加到返回数据集中
    return retDataSet                       #返回数据集


def chooseBestFeatureToSplit(dataSet):  #定义语法
    numFeatures = len(dataSet[0]) - 1  # 因为数据集的最后一项是标签（获取特征数，不计标签列）
    baseEntropy = calcShannonEnt(dataSet)#计算出原始数据集的信息熵
    bestInfoGain = 0.0
    bestFeature = -1                    #初始化最优增益和最优的特征编号
    for i in range(numFeatures):    #遍历所有特征
        featList = [example[i] for example in dataSet]  #创建对应特征的所有数据
        uniqueVals = set(featList)      #获得去重复后的集合，使用set去重
        newEntropy = 0.0                #初始化新信息熵为0
        for value in uniqueVals:           #遍历一列的value集合
            subDataSet = splitDataSet(dataSet, i, value)    #获得第i列值为value的数据集
            prob = len(subDataSet) / float(len(dataSet))    #计算概率
            newEntropy += prob * calcShannonEnt(subDataSet) #累计计算信息熵
        infoGain = baseEntropy - newEntropy         #计算信息增益
        if infoGain > bestInfoGain:     #如果信息增益大于最好信息增益
            bestInfoGain = infoGain     #找到新的最好信息增益
            bestFeature = i         #找出新的最优特征列序号
    return bestFeature          #返回最优特征列序号


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
    del (labels[bestFeat])      #将已选维度标签从标签列表中删除
    featValues = [example[bestFeat] for example in dataSet] #取出最优列的数据值
    uniqueVals = set(featValues)    #对最优列的值去重
    for value in uniqueVals:    #遍历最优列的每个值
        subLabels = labels[:]  # 为了不改变原始列表的内容复制了一下
        myTree[bestFeatLabel][value] = createTree(splitDataSet(dataSet,bestFeat, value), subLabels) #对分割子集，并构建子树结构
    return myTree       #返回构建的树结构
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

#分类算法代码
def classify(inputTree, featLabels, testVec):   #定义语法
    firstStr = list(inputTree.keys())[0]    #获取树的根节点的key值
    secondDict = inputTree[firstStr]    #将第一个节点值存到secondDict字典中
    featIndex = featLabels.index(firstStr)  #判断根节点名称获取在标签中的先后顺序
    key = testVec[featIndex]    #获取测试数据的键值
    valueOffset = secondDict[key]
    print ('+++', firstStr, 'xxx', secondDict, '---', key, '>>>', valueOffset)
    if isinstance(valueOffset, dict):
        classLabel = classify(valueOffset, featLabels, testVec)
    else:
        classLabel = valueOffset
    return classLabel




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
# print(classify(myTree, mylabel, [1, 1]))
createPlot(myTree)


def main():
    data, label = createDataSet()
    myTree = createTree(data, label)
    print(myTree)


if __name__ == '__main__':
    main()