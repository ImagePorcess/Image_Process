import numpy as np
import cv2

"""
对比度增强常用方法：
线性变换:liner_transform
分段线性变换: base on liner_transform
伽马变换: gamma_transform
直方图正规化: hist_normal
直方图均衡化：cv2.equalHist
局部自适应直方图均衡化：cv2.createCLAHE
"""


# 线性变换
def liner_transform(src, a, b):
    assert isinstance(a, float)  # 必须是浮点型，否则（np.uint8）会发生取余数操作
    trans = src * a + b
    trans[trans > 255] = 255
    trans = np.around(trans)
    trans = trans.astype(np.uint8)
    return trans


# 直方图正规化
def hist_normal(gray_src):
    # 输入的最小灰度级和最大灰度级
    in_max, in_min, _, _ = cv2.minMaxLoc(gray_src)
    # 要输出的最小灰度级和最大灰度级
    out_max, out_min = 255, 0
    # 计算a和b的值
    a = float(out_max - out_min) / (in_max - in_min)
    b = out_min - a * in_min
    # 矩阵的线性变换
    out = a * gray_src + b
    # 数据类型转换
    out = out.astype(np.uint8)
    return out


# 伽马变换
def gamma_transform(gray_src, gamma=0.3):
    # 图像归一化
    normal = gray_src / 255.0
    out = np.power(normal, gamma)
    return out

