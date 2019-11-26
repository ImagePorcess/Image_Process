import numpy as np


# 傅里叶变换
def fft_trans(src, alpha=20):
    f = np.fft.fft2(src)  # 傅里叶变换
    fshift = np.fft.fftshift(f)  # 零频率移到中心
    result = alpha * np.log(np.abs(fshift))  # 阈值转换
    return result


# 逆傅里叶变换
def ifft_trans(src):
    f = np.fft.fft2(src)
    fshift = np.fft.fftshift(f)  # 傅里叶变换
    ishift = np.fft.ifftshift(fshift)
    result = np.fft.ifft2(ishift)
    result = np.abs(result)  # 逆傅里叶变换
    return result
