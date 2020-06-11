// TestOperator.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include <iostream>
#include "opencv/include/opencv2/highgui.hpp"
#include "opencv/include/opencv2/opencv.hpp"
#include <vector>
#include <stdio.h> 
#include "Test.h"
#include "a.h"
//#include "NumCpp.hpp"
//using namespace nc;
using namespace cv; 
using namespace std;

int g_dAlphaValue;
int g_dBetaValue;
Mat g_srcImage1, g_srcImage2, g_dstImage;
double g_dFactor;
void on_Trackbar(int, void*)//响应滑动条的回调函数
{
	//double Value1 = (double) g_dAlphaValue / 100; //当前Alpha相对于最大值所占比例（global，double）
	//double Value2 = (1.0 - Value1); //当前Beta相对于最大值所占比例（global，double）
	//addWeighted(g_srcImage1, Value1, g_srcImage2, Value2, 0, g_dstImage);//图像的叠加
	//imshow("srcImage", g_dstImage);//在指定窗口显示图像
	//saturate_cast是防止数据溢出， >= 255 =255   <=0 = 0;
	namedWindow("src", 1);
	for (int y = 0; y < g_srcImage1.rows; y++)
	{
		for (int x = 0; x < g_srcImage1.cols; x++)
		{
			for (int c = 0; c < 3; c++)
			{
				g_dstImage.at<Vec3b>(y, x)[c] = saturate_cast<uchar>((g_dAlphaValue*0.01)*(g_srcImage1.at<Vec3b>(y, x)[c]) + g_dBetaValue);
			}
		}
	}
	imshow("src", g_srcImage1);
	imshow("dst", g_dstImage);

}
 
void on_Trackbar1(int, void*)//响应滑动条的回调函数
{
	////g_dAlphaValue = (double)g_dAlphaValue / 100; //当前Alpha相对于最大值所占比例（global，double）
	////g_dBetaValue = (1.0 - g_dAlphaValue); //当前Beta相对于最大值所占比例（global，double）
	////addWeighted(g_srcImage1, g_dAlphaValue, g_srcImage2, g_dBetaValue, 0, g_dstImage);//图像的叠加
	////imshow("srcImage", g_dstImage);//在指定窗口显示图像

	
}

void on_Trackbar2(int, void*) // gamma 的回调函数
{
	//建立查找表
	namedWindow("dst", 1);
	g_dFactor = (double) g_dAlphaValue / 50;
	unsigned char LUT[256];
	for (int i = 0; i < 256; i++)
	{ 
		LUT[i] = saturate_cast<uchar>(pow((float)(i / 255.0), g_dFactor)* 255.0f);
	}
	 
	g_dstImage = g_srcImage1.clone();

	if (g_dstImage.channels() == 1)
	{
		MatIterator_<uchar> iteratorBeg = g_dstImage.begin<uchar>();
		MatIterator_<uchar> iteratorEnd = g_dstImage.end<uchar>();

		for (; iteratorBeg < iteratorEnd; iteratorBeg++)
		{
			*iteratorBeg = LUT[(*iteratorBeg)];  
		}
	}
	else   
	{
		MatIterator_<cv::Vec3b> iteratorBeg =
			g_dstImage.begin<Vec3b>();
		MatIterator_<cv::Vec3b> iteratorEnd =
			g_dstImage.end<Vec3b>();
		//  通过查找表进行转换
		for (; iteratorBeg < iteratorEnd; iteratorBeg++)
		{
			(*iteratorBeg)[0] = LUT[((*iteratorBeg)[0])];
			(*iteratorBeg)[1] = LUT[((*iteratorBeg)[1])];
			(*iteratorBeg)[2] = LUT[((*iteratorBeg)[2])];
		}
	}

	imshow("dst", g_dstImage);
}

int _tmain(int argc, _TCHAR* argv[])
{

	//ORB 找寻特征点
	//Mat srcPic = imread("D:\\WRX\\X-ray\\1030\\1.bmp");
	//Mat dstPic = srcPic.clone();

	//imshow("src", srcPic);
	//auto orb_detector = ORB::create(1000);

	//vector<KeyPoint> kpts;

	//// 开启ORB检查
	//orb_detector->detect(srcPic, kpts);

	//// 绘制关键点
	//drawKeypoints(srcPic, kpts, dstPic, Scalar(0, 0, 255), DrawMatchesFlags::DEFAULT);
	//imshow("dst", dstPic);
	//ORB 找寻特征点
	 
	//角点检测
	//Mat srcPic = imread("D:\\WRX\\X-ray\\1030\\1.bmp");
	//Mat src;
	//cvtColor(srcPic, src, CV_RGB2GRAY);
	//imshow("src", src);
	//Mat dstPic = Mat::zeros(srcPic.size(), CV_32FC1);
	//cornerHarris(src, dstPic, 3, 3, 0.01);
	//threshold(dstPic, dstPic, 0.0001, 255, THRESH_BINARY);
	///*normalize(dstPic, norm_dst, 0, 255, NORM_MINMAX, CV_32FC1, Mat());
	//convertScaleAbs(norm_dst, normScaleDst);*/
	//imshow("dst", dstPic);


	//Laplacian高通滤波算子
	/*Mat input = imread("D:\\WRX\\X-ray\\1030-1\\16-1.bmp");
	Mat h1_kernel = (Mat_<char>(3, 3) << -1, -1, -1, -1, 8, -1, -1, -1, -1);
	Mat h2_kernel = (Mat_<char>(3, 3) << 0, -1, 0, -1, 5, -1, 0, -1, 0);

	Mat h1_result, h2_result;
	filter2D(input, h1_result, CV_32F, h1_kernel);
	filter2D(input, h2_result, CV_32F, h2_kernel);
	convertScaleAbs(h1_result, h1_result);
	convertScaleAbs(h2_result, h2_result);

	imshow("h1_result", h1_result);
	imshow("h2_result", h2_result);*/


	//Unshrpen Mask算法
	/*Mat input = imread("D:\\WRX\\X-ray\\1030-1\\16-1.bmp");
	Mat blur, usm;
	GaussianBlur(input, blur, Size(0, 0), 25);
	addWeighted(input, 1.5, blur, -0.5, 0, usm);
	imshow("usm", usm);*/
	//Unshrpen Mask算法
	
	//canny算子提取直线和轮廓
	/*Mat input = imread("D:\\WRX\\X-ray\\1030\\1.bmp");
	Mat output;
	Canny(input, output, 350, 400);
	imshow("dst", output);*/
	//canny算子提取直线和轮廓

	//创建拖动条
	//g_srcImage1 = imread("D:\\WRX\\X-ray\\1030\\1.bmp");
	//g_srcImage2 = imread("D:\\WRX\\X-ray\\1030-1\\16-1.bmp");
	//namedWindow("srcImage");
	//g_dAlphaValue = 10;
	//char TrackbarName[50];//声明滑动条的名称存储变量
	//sprintf_s(TrackbarName, "透明度%d", 100);
	//createTrackbar("TrackbarName", "srcImage", &g_dAlphaValue, 100, on_Trackbar);
	//createTrackbar("TrackbarName1", "srcImage", &g_dAlphaValue, 100, on_Trackbar);

	//on_Trackbar(g_dAlphaValue, 0);//结果在回调函数中显示
	//创建拖动条

	//使用拖动条调节亮度和对比度
	/*g_srcImage1 = imread("D:\\WRX\\X-ray\\1030\\1.bmp");
	g_dstImage = Mat::zeros(g_srcImage1.size(), g_srcImage1.type());
	g_dAlphaValue = 80;
	g_dBetaValue = 80;

	namedWindow("dst", WINDOW_AUTOSIZE);

	createTrackbar("对比度", "dst", &g_dAlphaValue, 300, on_Trackbar);
	createTrackbar("亮度", "dst", &g_dBetaValue, 200, on_Trackbar);

	on_Trackbar(g_dAlphaValue, 0);
	on_Trackbar(g_dBetaValue, 0);*/
	//使用拖动条调节亮度和对比度


	//gamma变换
	g_dAlphaValue = 15;
	g_srcImage1 = imread("D:\\WRX\\X-ray\\1030\\1.bmp");
	namedWindow("dst", WINDOW_AUTOSIZE);
	createTrackbar("gamma", "dst", &g_dAlphaValue, 100, on_Trackbar2);
	on_Trackbar2(g_dAlphaValue, 0);
	//gamma变换


	//高斯函数差分DOG
	//Mat srcPic = imread("D:\\WRX\\X-ray\\1030\\1.bmp");
	//Mat dstPic, g1, g2, dogImg;
	//cvtColor(srcPic, dstPic, CV_RGB2GRAY);
	////两次高斯模糊
	//GaussianBlur(srcPic, g1, Size(3, 3), 0, 0);
	//GaussianBlur(g1, g2, Size(3, 3), 0);
	//subtract(g1, g2, dogImg, Mat());   //差分图的灰度值比较小，图比较暗。

	////归一化显示
	//normalize(dogImg, dogImg, 255, 0, NORM_MINMAX);  //归一化，放到0-255显示。
	//imshow("DOG_img", dogImg);
	////DOG结束
	//Mat dstPic1, srcPic1, norm_dst, normScaleDst;
	//cvtColor(dogImg,srcPic1 , CV_RGB2GRAY);
	//cornerHarris(srcPic1, dstPic1, 3, 3, 0.01);
	//threshold(dstPic1, dstPic1, 0.0001, 255, THRESH_BINARY);
	//normalize(dstPic, norm_dst, 0, 255, NORM_MINMAX, CV_32FC1, Mat());
	//convertScaleAbs(norm_dst, normScaleDst);

	//for (int j = 0; j < norm_dst.rows; j++)
	//{
	//	for (int i = 0; i < norm_dst.cols; i++)
	//	{
	//		if ((int)norm_dst.at<float>(j, i) < 140 && (int)norm_dst.at<float>(j, i) > 130)//在满足阈值条件的角点处画圆；

	//		{
	//			circle(normScaleDst, Point(i, j), 5, Scalar(0, 0, 255), 2, 8);
	//		}
	//	}
	//}
	//imshow("dst", normScaleDst);
	//高斯函数差分DOG+角点检测获取特征点


	//判断图片为纯白/黑图
	/*g_srcImage1 = imread("D:\\WRX\\X-ray\\1030\\black.bmp");
	IplImage src = IplImage(g_srcImage1);

	if ((int)cvAvg(&src).val[0] > 254)
	{
		cout << "empty pic" << endl;
	}

	if ((int)cvAvg(&src).val[0] <= 10)
	{
		cout << "black pic" << endl;
	} 
	imshow("src", g_srcImage1);	 */

	/*cv::Point origin;
	string text;
	int font_face = cv::FONT_HERSHEY_COMPLEX;
	double font_scale = 2;
	int thickness = 2;
	int baseline;
	Size text_size = getTextSize(text, font_face, font_scale, thickness, &baseline);

	g_srcImage1 = imread("D:\\WRX\\X-ray\\1030\\empty.bmp");
	cv:Scalar tempVal = cv::mean(g_srcImage1);
	float matMean = tempVal.val[0];
	origin.x = g_srcImage1.cols / 2 - text_size.width / 2;
	origin.y = g_srcImage1.rows / 2 + text_size.height / 2;

	if (matMean > 254.00)
	{
		text = "white image";
		putText(g_srcImage1, text, origin, font_face, font_scale, cv::Scalar(0, 255, 255), thickness, 8, 0);
	}
	else if (matMean < 11.00)
	{
		text = "black image";
		putText(g_srcImage1, text, origin, font_face, font_scale, cv::Scalar(0, 255, 255), thickness, 8, 0);
	}
	imshow("src", g_srcImage1);*/

	
	//判断图片为纯白/黑图

	//选择感性区域
	//g_srcImage1 = imread("D:\\WRX\\X-ray\\1030\\1.bmp");
	//g_srcImage2 = g_srcImage1(Rect(0, 0, 300, 300)); // 浅拷贝
	//Mat img1;
	//g_srcImage2.copyTo(img1);//深拷贝
	//namedWindow("src",1);
	//namedWindow("dst1",1);
	//namedWindow("dst2",1);
	//circle(g_srcImage2, Point(10, 10), 5, Scalar(0, 0, 255), 2, 8);

	//imshow("dst1", g_srcImage2);

	//imshow("dst2", img1);

	//imshow("src", g_srcImage1);
	//选择感性区域时,使用浅拷贝可以降低检测图像尺寸,同时把检测数据保存到原图上

	//uint8 inMinValue, inMaxValue;
	
	//auto cvArray = Mat(g_srcImage1);
	//auto transposedCvArray = cv::Mat(cvArray.cols, cvArray.rows, CV_8SC1);
	//transpose(cvArray, transposedCvArray);
	//NdArray<uint8> inArray = NdArray<uint8>(transposedCvArray.data, transposedCvArray.rows, transposedCvArray.cols);
	//clip(&inArray, inMinValue, inMaxValue);
	waitKey(0);
	return 0;
}