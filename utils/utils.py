import cv2
import numpy as np


# 展示图像
def show_image(name, src, width=1000, height=1000):
    cv2.namedWindow(name, cv2.WINDOW_NORMAL)
    cv2.resizeWindow(name, width, height)
    cv2.imshow(name, src)
    cv2.waitKey(0)
    cv2.destroyAllWindows()


# 将数值标准化
def float2uint8(src):
    trans = np.clip(src, 0, 255).astype(np.uint8)
    return trans


# 计算图像梯度
def cal_attitude(src):
    sobelx = cv2.Sobel(src, cv2.CV_64F, 1, 0, ksize=3)
    sobely = cv2.Sobel(src, cv2.CV_64F, 0, 1)
    gm = cv2.sqrt(sobelx ** 2 + sobely ** 2)
    gm = np.array(gm, dtype=np.uint8)
    return gm


# 创建直方图
def image_hist(image):
    hist = cv2.calcHist([image], [0], None, [256], [0, 256])
    return hist


# 直方图比较
def hist_compare(image1, image2):
    hist1 = image_hist(image1)
    hist2 = image_hist(image2)
    # 巴氏距离
    match1 = cv2.compareHist(hist1, hist2, cv2.HISTCMP_BHATTACHARYYA)
    # 相关性
    match2 = cv2.compareHist(hist1, hist2, cv2.HISTCMP_CORREL)
    # 卡方
    match3 = cv2.compareHist(hist1, hist2, cv2.HISTCMP_CHISQR)
    # 巴氏距离越小越相似，相关性越大越相似，卡方越大越不相似
    print("巴氏距离", match1)
    print("相关性", match2)
    print("卡方", match3)


# 将计算结果标准化
def double2uint8(I, ratio=1.0):
    return np.clip(np.round(I * ratio), 0, 255).astype(np.uint8)