import numpy as np
import cv2


# 展示图像
def show_image(name, src, width=1000, height=1000):
    cv2.namedWindow(name, cv2.WINDOW_NORMAL)
    cv2.resizeWindow(name, width, height)
    cv2.imshow(name, src)
    cv2.waitKey(0)
    cv2.destroyAllWindows()


# 将最后的矩阵中的元素归一化到0~1之间
def minmaxscaler(data):
    min = np.min(data)
    max = np.max(data)
    return (data - min) / (max - min)


# 参数定义
INIT_SIGMA = 0.5
INTERVALS = 3


# 构建尺度空间
# 灰度层处理
def CreateSmoothGray(src, sigma):
    gray = cv2.cvtColor(src, cv2.COLOR_BGR2GRAY)
    resize = cv2.resize(gray, (gray.shape[0] * 2, gray.shape[1] * 2))
    sigma_init = np.sqrt(np.power(sigma, 2) - np.power((INIT_SIGMA * 2), 2))
    gauss = cv2.GaussianBlur(resize, (3, 3), sigmaX=sigma_init, sigmaY=sigma_init)
    return gauss


# 计算高斯金字塔的组数
def cal_octaves(src):
    octaves = int(np.log2(min(src.shape[0], src.shape[1]))) - 3
    return octaves


# 高斯金字塔实现
def GaussPyramid(src, octaves, intevals, sigma):
    sigmas = [sigma]
    dst = src.copy()
    gauss_pyr = []
    k = np.power(2, 1.0 / intevals)
    for i in range(1, intevals + 3):
        sig_prev = np.power(k, i - 1) * sigma
        sig_total = sig_prev * k
        sig_init = np.sqrt(np.power(sig_total, 2) - np.power((sig_prev * 2), 2))
        sigmas.append(sig_init)
    for o in range(octaves):
        for i in range(intevals + 3):
            if o == 0 & i == 0:
                dst = dst
            elif i == 0:
                dst = cv2.resize(dst, (dst.shape[0] // 2, dst.shape[1] // 2))
            else:
                dst = cv2.GaussianBlur(dst, (3, 3), sigmaX=sigmas[i], sigmaY=sigmas[i])
            gauss_pyr.append(dst)
    return gauss_pyr


# DOG金字塔实现
def DogPyramid(gauss_pyr, intervals):
    octaves = len(gauss_pyr) // (intervals + 3)
    dog_pyr = []
    for o in range(octaves):
        for i in range(intervals + 2):
            src1 = gauss_pyr[o * (intervals + 3) + i]
            src2 = gauss_pyr[o * (intervals + 3) + i + 1]
            sub = cv2.subtract(src1, src2, dtype=cv2.CV_8U)
            dog_pyr.append(sub)
    return dog_pyr


if __name__ == '__main__':
    # 读取图像
    src = cv2.imread(r"D:\work\libs\image\1.bmp")
    # 预处理完的图像
    gray = CreateSmoothGray(src, 1.6)
    # 返回金字塔的组数
    octaves = cal_octaves(gray)
    # 返回高斯金字塔
    gauss_pyr = GaussPyramid(gray, octaves, 3, 0.5)
    # 返回DOG金字塔
    dog_pyr = DogPyramid(gauss_pyr, 3)
    # 循环遍历查看结果
    for i in range(len(dog_pyr)):
        dog_pyr[i] = minmaxscaler(dog_pyr[i])
        show_image("result", dog_pyr[i])
