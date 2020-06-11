// TestOperator.cpp : 定义控制台应用程序的入口点。
//


// 电芯检测流程
// step1：先判断是否为黑白图（解决）
// step2：检测电芯阴阳极结构
// step3：计算电芯阴阳极落差值（overhang）和最值，均值
// step4：保存图像数据

// 拆解step2
// 1：获取检查区域 - 获得电极的起始边界
// 2：计算电极的位置和宽度 
// 3：获取阴阳极片的边缘轮廓图
// 4：获取阴阳极极片边缘和端点位置
// 5：获取第一个电极之后的电极与背景的分割线
// 6：获取阴极阳极的分割线位置
// 7：获取阴极阳极端点（用来计算overhang） - 根据计算结果判断OK/NG 

#include "stdafx.h"
#include <iostream>
#include "opencv/include/opencv2/highgui.hpp"
#include "opencv/include/opencv2/opencv.hpp"
#include "opencv/include/opencv2/ml/ml.hpp"
#include <vector>
#include <stdio.h> 
#include "Test.h"
#include "a.h"
#include <ctime>
#include <fstream>
#include <io.h>
//#include "NumCpp.hpp"
//using namespace nc;
using namespace cv; 
using namespace std;
using namespace cv::ml;

int g_dAlphaValue;
int g_dBetaValue;
int g_dDeltValue;
int g_dGamaValue;
Mat g_srcImage1, g_srcImage2, g_dstImage;
Mat g_srcThode, g_srcCathode;
double g_dFactor;
double g_dFactor2;
vector<Point2f> thode;
vector<Point2f> thode2;
vector<Point2f> cathode;
vector<int> border;

string mStrModelName;
bool mIsInit = false;
cv::Ptr<cv::ml::SVM> svm;
vector<Mat>mVecSamples;
vector<int>mVecLabels;

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



void on_TrackTomasTest(int , void * )
{
	vector<Point2f> thode;
	vector<Point2f> cathode;
	g_dFactor = (double)g_dAlphaValue / 100;
	g_dFactor2 = (double)4 / 100;
	int start_time = clock();
	double minDistance1 = g_dBetaValue;
	double minDistance2 = g_dGamaValue;
	int blocksize = 3, gardensize = 3 ;
	bool useHarries = false;
	bool useHarries2 = false;

	double k = 0.04;

	int posx = 0, posy = 0, posy1 = 0;
	//找到检测区域
	for (int i =  2 * g_srcImage1.cols / 3; i > 0; i--)
	{

		Scalar pos = g_srcImage1.at<uchar>(2 * g_srcImage1.cols / 3, i);
		if ( pos[0] < 230)
		{
			posx = i;
			break;
		}
		
	}

	for (int i = g_srcImage1.rows / 3; i < g_srcImage1.rows; i++)
	{
		Scalar pos = g_srcImage1.at<uchar>(i, g_srcImage1.cols / 2);
		if (pos[0] < 230)
		{
			posy = i;
			break;
		}
	}

	for (int i = posy; i < g_srcImage1.rows; i++)
	{
		Scalar pos = g_srcImage1.at<uchar>(i, g_srcImage1.cols / 2);
		if ( pos[0] <= 50 )
		{
			posy1 = i;
			break;
		}
	}

	g_dstImage = g_srcImage1(Rect(posx - 350, posy - 50, 400, 250)).clone();

	Mat g1, g2;
	g1 = g_dstImage(Rect(0, 0, g_dstImage.cols, posy1 - posy )); // 只取头部的部分
	//g2 = g_dstImage(Rect(0, g_dstImage.rows - 50 - (posy1 - posy) / 2, g_dstImage.cols, 50));
	imshow("g1", g1);
	goodFeaturesToTrack(g1, thode, 15, g_dFactor, minDistance1, Mat(), blocksize, useHarries, k);
	//goodFeaturesToTrack(g2, cathode, 10, g_dFactor2, minDistance2, Mat(), blocksize, useHarries2, k);

	int radius = 4;

	for (int i = 0; i < thode.size(); i++)
	{
		if (i == 0 || i == thode.size() - 1 )
		{
			circle(g1, thode[i], radius, Scalar(0, 0, 255), FILLED);
		}
		else if (abs(thode[i].x - thode[i - 1].x) > 15 && abs(thode[i].x - thode[i + 1].x) > 15)	// 筛选条件，暂定为区域筛选
		{
			circle(g1, thode[i], radius, Scalar(0, 0, 255), FILLED);
		}
	}

	
	//for (int i = 0; i < cathode.size(); i++)
	//{
	//	if (i == 0 || i == cathode.size() - 1)
	//	{
	//		circle(g2, cathode[i], radius, Scalar(0, 0, 255), FILLED);
	//	}
	//	else if (abs(cathode[i].x - cathode[i - 1].x) > 15 && abs(cathode[i].x - cathode[i + 1].x) > 15)	// 筛选条件，暂定为区域筛选
	//	{
	//		circle(g2, cathode[i], radius, Scalar(0, 0, 255), FILLED);
	//	}
	//}

	int end_time = clock();
	cout << end_time - start_time << endl;
	imshow("dst", g_dstImage);
}

//整体标定
//设置检查区域
//找到定位点
//图像分割,背景和检测区域分离
//通过图像每个像素点是否满足某阈值来确定背景和前景
//关键点是如何获得最佳阈值
//1直方图：根据波峰和波谷确定阈值,需要对比明显的图像
//2最大类间方差法AtsoThreshold,基于全局的二值化算法,根据灰度特性把图像分为前景和背景，当取最佳阈值两部分的差别最大
//
int AtsoThreshold(Mat &image)	//必须是灰度图
{
	if (image.channels() != 1)
	{
		cout << "必须输出灰度图！" << endl;

		return 0;
	}

	int T = 0; //Otsu算法阈值  
	double varValue = 0; //类间方差中间值保存
	double w0 = 0; //前景像素点数所占比例  
	double w1 = 0; //背景像素点数所占比例  
	double u0 = 0; //前景平均灰度  
	double u1 = 0; //背景平均灰度  
	double Histogram[256] = { 0 }; //灰度直方图，下标是灰度值，保存内容是灰度值对应的像素点总数  
	uchar *data = image.data;

	double totalNum = image.rows*image.cols; //像素总数

	for (int i = 0; i < image.rows; i++)
	{
		for (int j = 0; j < image.cols; j++)
		{
			if (image.at<uchar>(i, j) != 0)
			{
				Histogram[data[i*image.step + j]]++;
			}

		}
	}

	int minpos, maxpos;
	for (int i = 0; i < 255; i++)
	{
		if (Histogram[i] != 0)
		{
			minpos = i;
			break;
		}
	}
	for (int i = 255; i > 0; i--)
	{
		if (Histogram[i] != 0)
		{
			maxpos = i;
			break;
		}
	}


	for (int i = minpos; i <= maxpos; i++)
	{
		//每次遍历之前初始化各变量  
		w1 = 0;       u1 = 0;       w0 = 0;       u0 = 0;
		//***********背景各分量值计算**************************  
		for (int j = 0; j <= i; j++) //背景部分各值计算  
		{
			w1 += Histogram[j];   //背景部分像素点总数  
			u1 += j*Histogram[j]; //背景部分像素总灰度和  
		}
		if (w1 == 0) //背景部分像素点数为0时退出  
		{
			break;
		}
		u1 = u1 / w1; //背景像素平均灰度  
		w1 = w1 / totalNum; // 背景部分像素点数所占比例
		//***********背景各分量值计算**************************  

		//***********前景各分量值计算**************************  
		for (int k = i + 1; k < 255; k++)
		{
			w0 += Histogram[k];  //前景部分像素点总数  
			u0 += k*Histogram[k]; //前景部分像素总灰度和  
		}
		if (w0 == 0) //前景部分像素点数为0时退出  
		{
			break;
		}
		u0 = u0 / w0; //前景像素平均灰度  
		w0 = w0 / totalNum; // 前景部分像素点数所占比例  
		//***********前景各分量值计算**************************  

		//***********类间方差计算******************************  
		double varValueI = w0*w1*(u1 - u0)*(u1 - u0); //当前类间方差计算  
		if (varValue < varValueI)
		{
			varValue = varValueI;
			T = i;
		}
	}

	return T;
	//Mat dst;
	//threshold(image, dst, T, 255, CV_THRESH_OTSU);

	//std::vector<std::vector<cv::Point>> contours;
	//vector<Vec4i> hireachy;

	//findContours(dst, contours, hireachy, CV_RETR_TREE, CV_CHAIN_APPROX_NONE);

	//Mat result_image = Mat::zeros(dst.size(), CV_8UC3);

	//Scalar color[] = { Scalar(0, 0, 255), Scalar(0, 255, 0), Scalar(255, 0, 0), Scalar(255, 255, 0), Scalar(255, 0, 255) };

	////初始化周长、面积、圆形度、周径比
	//double len = 0, area = 0, roundness = 0, lenratio = 0;

	////循环找出所有符合条件的轮廓
	//for (size_t t = 0; t < contours.size(); t++)
	//{
	//	//条件：过滤掉小的干扰轮廓
	//	Rect rect = boundingRect(contours[t]);
	//	if (rect.width < 10)
	//		continue;
	//	//画出找到的轮廓
	//	drawContours(result_image, contours, static_cast<int>(t), Scalar(255, 255, 255), 1, 8, hireachy);

	//	//imshow("cont", result_image);
	//	//绘制轮廓的最小外结矩形
	//	/*RotatedRect minrect = minAreaRect(contours[t]);
	//	Point2f P[4];
	//	minrect.points(P);
	//	for (int j = 0; j <= 3; j++)
	//	{
	//	line(result_image, P[j], P[(j + 1) % 4], cv::Scalar(0, 0, 255), 1);
	//	}*/
	//	//cout << minrect.size << endl;//外接矩形尺寸

	//	/*Point2f center; float radius;
	//	minEnclosingCircle(contours[t], center, radius);
	//	circle(result_image, center, radius, color[t], 1);*/

	//}

	////显示结果
	//imshow("轮廓图", result_image);

	//return result_image;
	

	//int cols = 19;
	//int rows = 19;
	//Mat kernel = getStructuringElement(MORPH_RECT, Size(cols, rows), Point(cols / 2, rows / 2));
	//morphologyEx(splitRect[i], splitRect[i], CV_MOP_OPEN, kernel); //开运算
	//morphologyEx(splitRect[i], splitRect[i], CV_MOP_OPEN, kernel); //闭运算
}

//修正特征点的位置
void FixThodePoints()
{
	Mat input = g_srcThode.clone();
	Mat input_sobelx;
	GaussianBlur(input, input, Size(3, 3), 0, 0);
	Sobel(input, input_sobelx, -1, 1, 0, 3, 1, 1, BORDER_DEFAULT);
	medianBlur(input_sobelx, input_sobelx, 3);
	Mat input_thode, input_canny;
	Mat left_thode, right_thode;
	GaussianBlur(input_sobelx, input_thode, Size(3, 3), 0, 0);
	Mat kern = getStructuringElement(MORPH_ELLIPSE, Size(3, 3));
	erode(input_thode, input_thode, kern);
	Canny(input_thode, input_canny, 10, 50);
	imshow("input_canny", input_canny);
	vector<int>thode_border;

	int posx = 0;
	for (int i = 0; i < input_canny.cols; i++)
	{
		Scalar pos = input_canny.at<uchar>(4 * input_canny.rows / 5, i);
		if (pos[0] == 255)
		{
			thode_border.push_back(i);
		}
	}

	posx = thode_border[0];

	left_thode = input_thode(Rect(posx - 1, 0, (input_thode.cols - posx) / 2, input_thode.rows));
	right_thode = input_thode(Rect((input_thode.cols + posx) / 2, 0, (input_thode.cols - posx) / 2, input_thode.rows));

	Canny(left_thode, left_thode, 10, 50);
	Canny(right_thode, right_thode, 60, 100);
	imshow("input_thode", input_thode);
	//把不在端点的点移动到端点上去


	//把不在边缘的点移动到边缘上
	for (vector<Point2f>::iterator it = thode.begin(); it != thode.end(); it++)
	{
		Scalar pos = input_thode.at<uchar>(it->y, it->x);
		if (pos[0] != 255)
		{
			bool flag = false;
			for (int i = it->x - 5; i < it->x + 5; i++)
			{
				Scalar pos1 = input_thode.at<uchar>(it->y, i);
			
				if (pos1[0] == 255)
				{
					it->x = i;
					flag = true;
					break;
				}
			}

			if ( flag == false ) // 没有找到边缘,就直接把点定到边缘的最低处,由下一步再移动到顶端上
			{
				int fixPos = it->x;
				int minPos = it->x;
				for (int i = 0; i < thode_border.size(); i++)
				{
					if (abs(thode_border[i] - it->x) > minPos)
					{
						minPos = abs(thode_border[i] - it->x);
						fixPos = thode_border[i];
					}
				}

				it->x = fixPos;
				it->y = input_thode.rows - 1;

			}
		}
	}

	for (int i = 0; i < thode.size(); i++)
	{
		Scalar pos = input_thode.at<uchar>(thode[i].y, thode[i].x);
		cout << thode[i].x << "," << thode[i].y << ":" << pos[0] << endl;
	}

	//再把不在顶端的点移动到顶端上
	for (vector<Point2f>::iterator it = thode.begin(); it != thode.end(); it++)
	{
		for (int j = it->y; j > 0; j--)
		{
			Scalar pos = input_thode.at<uchar>(j - 1, it->x);
			Scalar pos1 = input_thode.at<uchar>(j - 1, it->x - 1);
			Scalar pos2 = input_thode.at<uchar>(j - 1, it->x + 1);
			if (pos[0] == 255)
			{
				it->y = j;
				continue;
			}
			else if (pos1[0] == 255 )
			{
				it->y = j;
				it->x -= 1;
			}
			else if (pos2[0] == 255)
			{
				it->y = j;
				it->x += 1;
			}
		}
	}


	////排除不在边缘上的点
	for (vector<Point2f>::iterator it = thode.begin(); it != thode.end();)
	{
		Scalar pos = input_thode.at<uchar>(it->y, it->x);
		
		if (pos[0] != 255)
		{
			cout << it->x << "," << it->y << ":" << pos[0] << endl;
			it = thode.erase(it);
		}
		else
		{
			it++;
		}
	}

}

// 确定检测区域
void GetDetectPoleArea()
{

	Mat imgThreshold = g_srcImage1.clone();

	// 先对图像2值化处理
	threshold(imgThreshold, imgThreshold, 230, 255, THRESH_BINARY); //CV_THRESH_OTSU
	
	//找到坐标（posx,posy）
	int posx = 0, posy = 0,posy1 = 0;
	for (int i = 0; i < imgThreshold.rows; i++)
	{
		Scalar pos = imgThreshold.at<uchar>(i, imgThreshold.rows / 5);
		if ( pos[0] == 0)
		{
			posy = i;
			break;
		}
	}

	for (int i = imgThreshold.cols - 1; i > 0; i--)
	{
		Scalar pos = imgThreshold.at<uchar>(4 * imgThreshold.cols / 5, i);
		if (pos[0] == 0)
		{
			posx = i;
			break;
		}
	}

	//Point keypoint(posx, posy); // 图像参考点,该点的坐标位置是在g_srcImage1上的
	//circle(g_srcImage1, keypoint, 4, Scalar(0, 0, 255), FILLED);
	//imshow("g1", g_srcImage1);

	//根据参考点截取图像(原来)
	//g_srcImage2 = g_srcImage1(Rect(posx - 400, posy - 50, 450 , imgThreshold.rows / 6));
	//新
	//g_srcImage2 = g_srcImage1(Rect(posx - 700, posy - 50, 750, imgThreshold.rows / 3));
	g_srcImage2 = g_srcImage1(Rect(posx - 600, posy - 50, 650, imgThreshold.rows / 3));

	//根据灰度值找图像内层阳极区域参考点,该灰度值很关键,如果精确找到灰度值可以帮助增强算法的鲁棒性
	int _iGrayLevel = 0;
	Scalar pos = g_srcImage2.at<uchar>(g_srcImage2.rows - 1, g_srcImage2.cols / 2);
	_iGrayLevel = pos[0] + 10;


	for (int i = 0; i < g_srcImage2.rows; i++)
	{
		Scalar pos = g_srcImage2.at<uchar>(i, g_srcImage2.cols / 2);
		if (pos[0] <= _iGrayLevel)
		{
			posy1 = i;
			break;
		}
	}

	///*Point keypoint1(g_srcImage1.cols / 5, posy1);
	//circle(g_srcImage2, keypoint1, 2, Scalar(0, 0, 255), FILLED);*/
	//
	////根据2个参考点Y轴方向的差值，把阴极分离，方便Tomas找点
	g_srcThode = g_srcImage2(Rect(0, 0, g_srcImage2.cols, posy1 / 2 + 40));

	g_srcCathode = g_srcImage2(Rect(0, posy1 / 2 + 40, g_srcImage2.cols, g_srcImage2.rows - posy1 / 2 - 50));

	//imshow("Thode", g_srcThode);
}

void ScreenThode()
{
	//对点做对比,排除一些重复的点,排除与角点同X轴但是不同高的点
	for (int i = 0; i < thode.size(); i++)
	{

		for (int j = i + 1; j < thode.size(); j++)
		{
			if (abs(thode[i].x - thode[j].x) < 15)
			{
				if (thode[i].y < thode[j].y)
				{
					thode[j].x = 0;
					thode[j].y = 0;
				}
				else
				{
					thode[i].x = 0;
					thode[i].y = 0;
				}
			}
		}
	}

	for (vector<Point2f>::iterator it = thode.begin(); it != thode.end();)
	{
		if (it->x == 0)
		{
			it = thode.erase(it);
		}
		else
		{
			it++;
		}
	}

}

void ScreenMinBorderPoint()
{
	//再排除超出边缘的点
	Mat input = g_srcThode.clone();
	Mat input_sobelx, erode_thode;
	GaussianBlur(input, input, Size(3, 3), 0, 0);
	Sobel(input, input_sobelx, -1, 1, 0, 3, 1, 1, BORDER_DEFAULT);
	medianBlur(input_sobelx, input_sobelx, 3);
	Mat input_thode;
	GaussianBlur(input_sobelx, input_thode, Size(3, 3), 0, 0);
	Mat kern = getStructuringElement(MORPH_ELLIPSE, Size(3, 3));
	erode(input_thode, erode_thode, kern);
	Canny(erode_thode, input_thode, 2, 55);

	int minBorder = input_thode.cols;
	for (int i = 0; i < input_thode.cols; i++)
	{
		Scalar pos = input_thode.at<uchar>(input_thode.rows - 1, i);
		if (pos[0] == 255)
		{
			minBorder = i;
			break;
		}
	}

	for (vector<Point2f>::iterator it = thode.begin(); it != thode.end();)
	{
		if (it->x < minBorder - 10 )
		{
			it = thode.erase(it);
		}
		else
		{
			it++;
		}
	}


	// 确定某点是否是边缘点
	//for (vector<Point2f>::iterator it = thode.begin(); it != thode.end();)
	//{
	//	bool flag = true;
	//	for (int i = it->y; i < input_thode.rows - 1; i++)
	//	{
	//		Scalar pos = input_thode.at<uchar>(i, it->x);
	//		Scalar pos1 = input_thode.at<uchar>(i + 1, it->x);
	//		Scalar pos2 = input_thode.at<uchar>(i - 1, it->x);
	//		Scalar pos3 = input_thode.at<uchar>(i, it->x + 1);
	//		Scalar pos4 = input_thode.at<uchar>(i, it->x - 1);
	//		if (pos[0] == 0 && pos1[0] == 0 && pos2[0] == 0 && pos3[0] == 0 && pos4[0] == 0)
	//		{
	//			flag = false;
	//		}
	//	}

	//	if (flag == false)
	//	{
	//		it = thode.erase(it);
	//	}
	//	else
	//	{
	//		it++;
	//	}
	//}


	/*for (vector<Point2f>::iterator it = thode.begin(); it != thode.end();)
	{
		Scalar pos = input_thode.at<uchar>(input_thode.rows - 1, it->x);
		Scalar pos1 = input_thode.at<uchar>(input_thode.rows - 1, it->x + 1);
		Scalar pos2 = input_thode.at<uchar>(input_thode.rows - 1, it->x + 2);
		Scalar pos3 = input_thode.at<uchar>(input_thode.rows - 1, it->x - 1);
		Scalar pos4 = input_thode.at<uchar>(input_thode.rows - 1, it->x - 2);
		if (pos[0] == 0 && pos1[0] == 0 && pos2[0] == 0 && pos3[0] == 0 && pos4[0] == 0)
		{
			it = thode.erase(it);
		}
		else
		{
			it++;
		}
	}*/


	//imshow("dst", input_thode);
}

//排除由于折痕找到的角点
void ScreenTomsa()
{

	Mat input = g_srcThode.clone();
	Mat input_sobelx;
	GaussianBlur(input, input, Size(3, 3), 0, 0);
	Sobel(input, input_sobelx, -1, 1, 0, 3, 1, 1, BORDER_DEFAULT);
	medianBlur(input_sobelx, input_sobelx, 3);
	Mat input_thode, input_canny;
	Mat left_thode, right_thode;
	GaussianBlur(input_sobelx, input_thode, Size(3, 3), 0, 0);
	Mat kern = getStructuringElement(MORPH_ELLIPSE, Size(3, 3));
	erode(input_thode, input_thode, kern);
	Canny(input_thode, input_canny, 10, 50);
	
	int posx = 0;
	for (int i = 0; i < input_canny.cols; i++)
	{
		Scalar pos = input_canny.at<uchar>(4 * input_canny.rows / 5, i);
		if (pos[0] == 255)
		{
			posx = i;
			break;
		}
	}

	left_thode = input_thode(Rect(posx - 2, 0, (input_thode.cols - posx) / 2, input_thode.rows));
	right_thode = input_thode(Rect((input_thode.cols + posx) / 2, 0, (input_thode.cols - posx) / 2, input_thode.rows));

	Canny(left_thode, left_thode, 10, 50);
	Canny(right_thode, right_thode, 60, 100);

	for (vector<Point2f>::iterator it = thode.begin(); it != thode.end();)
	{
		Scalar pos = input_thode.at<uchar>(input_thode.rows - 1, it->x);
		Scalar pos1 = input_thode.at<uchar>(input_thode.rows - 1, it->x + 1);
		Scalar pos2 = input_thode.at<uchar>(input_thode.rows - 1, it->x + 2);
		Scalar pos3 = input_thode.at<uchar>(input_thode.rows - 1, it->x - 1);
		Scalar pos4 = input_thode.at<uchar>(input_thode.rows - 1, it->x - 2);
		if (pos[0] == 0 && pos1[0] == 0 && pos2[0] == 0 && pos3[0] == 0 && pos4[0] == 0)
		{
			bool flag = true;
			for (int i = it->x - 10; i < it->x + 10; i++)
			{
				Scalar pos = input_thode.at<uchar>(input_thode.rows - 1, i);
				Scalar pos1 = input_thode.at<uchar>(input_thode.rows - 1, i + 1);
				Scalar pos2 = input_thode.at<uchar>(input_thode.rows - 1, i + 2);
				Scalar pos3 = input_thode.at<uchar>(input_thode.rows - 1, i - 1);
				Scalar pos4 = input_thode.at<uchar>(input_thode.rows - 1, i - 2);

				if ( pos[0] == 255 )
				{
					it->x = i;
					flag = false;
					break;
				}
				else if (pos1[0] == 255)
				{
					it->x = i + 1;
					flag = false;
					break;
				}
				else if ( pos2[0] == 255)
				{
					it->x = i + 2;
					flag = false;
					break;
				}
				else if (pos3[0] == 255)
				{
					it->x = i - 1;
					flag = false;
					break;
				}
				else if (pos4[0] == 255)
				{
					it->x = i - 2;
					flag = false;
					break;
				}
			}

			if ( flag )
			{
				it = thode.erase(it);
			}
			else
			{
				it++;
			}
		}
		else
		{
			it++;
		}
	}
}

void SortThodePoints()
{

	for (int i = 0; i < thode.size(); i++)
	{
		for (int j = 0; j < thode.size() - i - 1; j++)
		{
			Point temp = thode[j];
			if ( thode[j].x > thode[j+1].x)
			{
				thode[j] = thode[j + 1];
				thode[j + 1] = temp;
			}
		}
	}

	for (vector<Point2f>::iterator it = thode.begin(); it + 1 != thode.end();)
	{
		if (abs(it->x - (it + 1)->x) <= 5 )
		{
			it = thode.erase(it);
		}
		else
		{
			it++;
		}
	}
}


void ClearScharr()
{
	/*Mat input = g_srcThode.clone();
	Mat input_thode, input_scharr, input_sobelx;
	GaussianBlur(input, input, Size(3, 3), 0, 0);
	Sobel(input, input_sobelx, -1, 1, 0, 3, 1, 1, BORDER_DEFAULT);
	Scharr(input, input_scharr, -1, 1, 0, 3, 0, BORDER_DEFAULT);
	threshold(input_sobelx, input_thode, 20, 255, THRESH_TOZERO);
	int length = 0;
	imshow("input_thode", input_thode);
	g_dFactor = 0.01;
	int start_time = clock();
	double minDistance1 = 21;
	int blocksize = 3, gardensize = 3;
	bool useHarries = false;
	double k = 0.04;
	int _iLayerNum = 20;
	goodFeaturesToTrack(input_thode, thode, _iLayerNum + 5, g_dFactor, minDistance1, Mat(), blocksize, useHarries, k);*/
	

	Mat input = g_srcThode.clone();
	Mat input_sobelx;
	GaussianBlur(input, input, Size(3, 3), 0, 0);
	Sobel(input, input_sobelx, -1, 1, 0, 3, 1, 1, BORDER_DEFAULT);
	medianBlur(input_sobelx, input_sobelx, 3);
	imshow("input_sobelx", input_sobelx);
	Mat input_thode, input_canny;
	GaussianBlur(input_sobelx, input_thode, Size(3, 3), 0, 0);
	Mat kern = getStructuringElement(MORPH_ELLIPSE, Size(3, 3));
	erode(input_thode, input_thode, kern);

	Canny(input_thode, input_canny, 10, 50);
	Mat left_thode, right_thode;

	int posx = 0;
	for (int i = 0; i < input_canny.cols; i++)
	{
		Scalar pos = input_canny.at<uchar>(4 * input_canny.rows / 5, i);
		if (pos[0] == 255)
		{
			posx = i;
			break;
		}
	}

	left_thode = input_thode(Rect(posx - 2, 0, (input_thode.cols - posx) / 2, input_thode.rows));
	right_thode = input_thode(Rect((input_thode.cols + posx) / 2, 0, (input_thode.cols - posx) / 2, input_thode.rows));

	Canny(left_thode, left_thode, 10, 50);
	Canny(right_thode, right_thode, 60, 100);
	imshow("left_thode", left_thode);
	imshow("right_thode", right_thode);
	imshow("input_canny", input_thode);  

}

void NewFindThodePoint()
{
	/*for (int i = 0; i < g_srcThode.cols; i++)
	{
		for (int j = 0; j < g_srcThode.rows / 2; j++)
		{
			g_srcThode.at<uchar>(j, i) = 255;
		}
	}*/
	cout << "进入查找阳极点2" << endl;
	Mat input = g_srcThode.clone();
	Mat input_sobelx,input_scharr;
	GaussianBlur(input, input, Size(3, 3), 0, 0);
	Sobel(input, input_sobelx, -1, 1, 0, 3, 1, 1, BORDER_DEFAULT);
	medianBlur(input_sobelx, input_sobelx, 3);
	imshow("input_sobelx", input_sobelx);
	Mat input_thode, input_canny;
	Mat left_thode, right_thode;
	GaussianBlur(input_sobelx, input_thode, Size(3, 3), 0, 0);
	Mat kern = getStructuringElement(MORPH_ELLIPSE, Size(3, 3));
	erode(input_thode, input_thode, kern);
	Canny(input_thode, input_canny, 10, 50);

	vector<int>thode_border;
	int posx = 0;
	for (int i = 0; i < input_canny.cols; i++)
	{
		Scalar pos = input_canny.at<uchar>(4 * input_canny.rows / 5, i);
		if (pos[0] == 255)
		{
			thode_border.push_back(i);
		}
	}

	posx = thode_border[0];

	left_thode = input_thode(Rect(posx - 1, 0, (input_thode.cols - posx) / 3, input_thode.rows));
	right_thode = input_thode(Rect((input_thode.cols + posx) / 3, 0, 2 * (input_thode.cols - posx) / 3, input_thode.rows));

	Canny(left_thode, left_thode, 10, 50);
	Canny(right_thode, right_thode, 60, 100);
	//imshow("input_thode", input_thode);

	Mat right_area, left_area;
	int right_posx = posx / 3 + input_thode.cols / 3;
	int right_posy_min = input_thode.rows, right_posy_max = 0;

	for (int i = (input_thode.cols + posx) / 3; i < input_thode.cols; i++)
	{
		for (int j = input_thode.rows - 1; j > 1; j--)
		{
			Scalar pos = input_thode.at<uchar>(j, i);
			Scalar pos1 = input_thode.at<uchar>(j - 1, i);
			if (pos[0] == 255)
			{
				if (j < right_posy_min)
				{
					right_posy_min = j;
				}

				if ( pos1[0] == 0)
				{
					if (j > right_posy_max)
					{
						right_posy_max = j;
					}
				}
			}

		}
	}

	right_area = input_thode(Rect((input_thode.cols + posx) / 3, right_posy_min - 10, 2 * (input_thode.cols - posx) / 3, (right_posy_max - right_posy_min)));

	//把不在端点的点移动到端点上去
	g_dFactor = 0.01;
	int start_time = clock();
	double minDistance1 = 21;
	int blocksize = 3, gardensize = 3;
	bool useHarries = false;
	double k = 0.04;
	int right_iLayerNum = 10;
	int left_iLayerNum = 3;
	goodFeaturesToTrack(right_area, thode, right_iLayerNum, g_dFactor, minDistance1, Mat(), blocksize, useHarries, k);
	goodFeaturesToTrack(left_thode, thode2, left_iLayerNum, g_dFactor, minDistance1, Mat(), blocksize, useHarries, k);

	imshow("left_thode", right_area);
	/*for (int i = 0; i < thode2.size(); i++)
	{
		thode2[i].x += posx;
		thode.push_back(thode2[i]);
		
	}*/

	for (int i = 0; i < thode.size(); i++)
	{
		thode[i].x += right_posx;
		thode[i].y += input_thode.rows / 3;
		circle(g_srcImage2, thode[i], 3, Scalar(0, 0, 255), FILLED);
	}

	imshow("dst", g_srcImage2);
	//把不在边缘的点移动到边缘上
	//for (vector<Point2f>::iterator it = thode.begin(); it != thode.end(); it++)
	//{
	//	Scalar pos = input_thode.at<uchar>(it->y, it->x);
	//	Scalar pos1 = input_thode.at<uchar>(it->y, it->x - 1);
	//	Scalar pos2 = input_thode.at<uchar>(it->y, it->x + 1);
	//	if (pos[0] != 255 && pos1[0] != 255 && pos2[0] != 255 )
	//	{
	//		int minPos = input_canny.cols;
	//		int origin = it->x;
	//		for (int i = 0; i < thode_border.size(); i++)
	//		{

	//			if (abs(origin - thode_border[i]) < minPos)
	//			{
	//				minPos = abs(it->x - thode_border[i]);
	//				it->x = thode_border[i];
	//			}
	//		}
	//	}
	//	
	//}

	//////再把不在顶端的点移动到顶端上
	//for (vector<Point2f>::iterator it = thode.begin(); it != thode.end(); it++)
	//{
	//	int posy = it->y;
	//	for (int j = it->y; j > 1; j--)
	//	{
	//		Scalar pos1 = input_thode.at<uchar>(j - 1, it->x);
	//		Scalar pos2 = input_thode.at<uchar>(j, it->x - 1);
	//		Scalar pos3 = input_thode.at<uchar>(j - 1, it->x - 1);
	//		Scalar pos4 = input_thode.at<uchar>(j, it->x + 1);
	//		Scalar pos5 = input_thode.at<uchar>(j - 1, it->x + 1);
	//		if (pos1[0] == 255)
	//		{
	//			it->y = j - 1;
	//		}
	//		else if (pos2[0] == 255)
	//		{
	//			it->x -= 1;
	//		}
	//		else if (pos3[0] == 255)
	//		{
	//			it->y = j - 1;
	//			it->x -= 1;
	//		}
	//		else if (pos4[0] == 255)
	//		{
	//			it->y = j;
	//			it->x += 1;
	//		}
	//		else if (pos5[0] == 255)
	//		{
	//			it->y = j - 1;
	//			it->x += 1;
	//		}

	//	}
	//}

	//for (int i = 0; i < thode.size(); i++)
	//{
	//	for (int j = 0; j < thode.size() - 1 - i; j++)
	//	{
	//		Point2f temp = thode[j];
	//		if (thode[j].x - thode[j + 1].x > 5)
	//		{
	//			thode[j] = thode[j + 1];
	//			thode[j + 1] = temp;
	//		}
	//		else if (abs(thode[j].x - thode[j + 1].x) >= 0 && abs(thode[j].x - thode[j + 1].x) <= 10)
	//		{
	//			if (thode[j].y > thode[j + 1].y)
	//			{
	//				thode[j] = thode[j + 1];
	//			}
	//			else
	//			{
	//				thode[j + 1] = thode[j];
	//			}
	//		}

	//	}
	//}

	////去掉数据相同的点
	//for (vector<Point2f>::iterator it = thode.begin(); it + 1 != thode.end();)
	//{
	//	if (abs(it->x - (it + 1)->x) <= 5)
	//	{
	//		it = thode.erase(it);
	//	}
	//	else
	//	{
	//		it++;
	//	}
	//}
	// 
}

void GetHOGFeature(Mat inputImg, Mat &outputImg)
{
	HOGDescriptor* descript = new HOGDescriptor(Size(64, 64), Size(16, 16), Size(8, 8), Size(8, 8), 9);
	vector<float> descriptValue;
	descript->compute(inputImg, descriptValue, Size(1, 1), Size(0, 0));
	outputImg = Mat(1, descriptValue.size(), CV_32FC1);
}

void FindFlodArea(Mat &src, Mat &desc)
{
	int nBins = 36;

	desc = Mat::zeros(1, nBins, CV_32FC1);

	Mat gradx, grady;
	Sobel(src, gradx, CV_32FC1, 1, 0, 3, 1, 1, BORDER_DEFAULT);
	Sobel(src, grady, CV_32FC1, 0, 1, 3, 1, 1, BORDER_DEFAULT);


	float oriStep = 360.0f / nBins;
	float *ptrDesc = desc.ptr<float>(0);

	for (int i = 0; i < src.rows; i++)
	{
		float* xData = gradx.ptr<float>(i);
		float* yData = grady.ptr<float>(i);

		for (int j = 0; j < src.cols; j++)
		{
			int bin = -1;
			float xval = xData[j];
			float yval = yData[j];
			float mag = std::sqrt(xval*xval + yval*yval);

			if (mag > 0)
			{
				float ori = std::acos(xval / mag) * 180.0f / CV_PI;
				if (yval < 0)
				{
					ori = 360.0f - ori;

				}
				bin = (int)(((int)(ori + oriStep / 2.0f) % 360) / oriStep);
			}

			if (bin >= 0)
				ptrDesc[bin] += mag;
		}
	}

	//cv::multiply(desc, mWeights, desc, 1.0, CV_32F);

	boxFilter(desc, desc, CV_32F, Size(5, 1));
	float norm = 0.0f;
	for (int i = 0; i < nBins; ++i)
		norm += ptrDesc[i];

	if (norm > 0)
	{
		for (int i = 0; i < nBins; ++i)
			ptrDesc[i] /= norm;
	} 

	svm->predict(desc);

}
// 判断褶皱,消除轻微褶皱,NG严重褶皱

bool InitSVM()
{
	mIsInit = true;
	svm = cv::ml::SVM::create();
	svm->setType(cv::ml::SVM::Types::C_SVC);
	svm->setKernel(cv::ml::SVM::KernelTypes::RBF);
	svm->setDegree(10.0);
	svm->setGamma(6.5);
	svm->setCoef0(1.5);
	svm->setC(10.0);
	svm->setNu(0.5);
	svm->setP(0.1);
	svm->setTermCriteria(cv::TermCriteria(cv::TermCriteria::MAX_ITER + cv::TermCriteria::EPS, 7500, 0.001));
	mStrModelName = std::string("D:\\Projects\\TestOperator\\x64\\Debug\\BrokenClassifier.xml");

	Mat mWeights = Mat::zeros(1, 36, CV_32FC1);

	float* weightData = mWeights.ptr<float>(0);

	float weights[36] =
	{
		0.5, 1.0, 1.0, 1.0, 1.0, 1.0,
		1.0, 1.0, 0.5, 1.0, 1.0, 1.0,
		1.0, 1.0, 1.0, 1.0, 1.0, 0.5,
		1.0, 1.0, 1.0, 1.0, 1.0, 1.0,
		1.0, 1.0, 0.5, 1.0, 1.0, 1.0,
		1.0, 1.0, 1.0, 1.0, 1.0, 0.5
	};

	for (int i = 0; i < mWeights.cols; i++)
	{
		weightData[i] = weights[i];
	}

	float weightSum = mean(mWeights).val[0];

	if (weightSum > 0)
	{
		mWeights.convertTo(mWeights, CV_32F, 1.0 / weightSum);
	}

	return 1;
}

void LoadSampleData(string strPath)
{
	/*vector<int>mVecLabels;*/
	vector<string>files;
	intptr_t   hFile = 0;//intptr_t和uintptr_t是什么类型:typedef long int/ typedef unsigned long int
	struct _finddata_t fileinfo;
	string p;
	if ((hFile = _findfirst(p.assign(strPath).append("\\*.bmp").c_str(), &fileinfo)) != -1)//assign方法可以理解为先将原字符串清空，然后赋予新的值作替换。
	{
		do
		{
			if (strcmp(fileinfo.name, ".") != 0 && strcmp(fileinfo.name, "..") != 0)
			{
				files.push_back(p.assign(strPath).append("\\").append(fileinfo.name));
				mVecSamples.push_back(imread(p, 0));
				string label = fileinfo.name;
				if ( label.rfind("OK") != string::npos )
				{
					mVecLabels.push_back(1);
					cout << "OK" << endl;
				}
				else
				{
					mVecLabels.push_back(-1);
				}
			}

		} while (_findnext(hFile, &fileinfo) == 0);
		_findclose(hFile);
	}

}

void readFileName()
{
	string strPath = "BrokenClassifier.xml";
	vector<string> vecFileName;
	vector<Mat> vecMat;
	vector<int> vecLabel;
	char separator = ';';
	vecFileName.clear();
	vecMat.clear();
	vecLabel.clear();
	ifstream infile(strPath.c_str(), ifstream::in);
	if (!infile)
	{
		string error_message = "No valid input file was given, please check the given filename.";
		CV_Error(CV_StsBadArg, error_message);
	}

	string line, path, classLabel;
	while (getline(infile, line))
	{
		stringstream liness(line);
		getline(liness, path, separator);
		getline(liness, classLabel);

		if (!path.empty() && !classLabel.empty())
		{
			Mat img = imread(path, 0);
			if (!img.empty())
			{
				vecFileName.push_back(path);
				vecMat.push_back(img);
				vecLabel.push_back(atoi(classLabel.c_str()));
			}
		}
	}
}


int getSampleFeature(vector<Mat>& vecMat, vector<int>& vecLabel, int nBins, Mat& descMat, Mat& labelMat)
{
	if ( vecMat.size() <= 0 || vecLabel.size() <=0 )
	{
		return 0;
	}

	int nSize = vecMat.size();
	if (!descMat.empty())
		descMat.release();
	if (!labelMat.empty())
		labelMat.release();

	for (int i = 0; i < nSize; i++)
	{
		Mat hogMat;
		FindFlodArea(vecMat[i], hogMat);
		descMat.push_back(hogMat);
		labelMat.push_back(vecLabel[i]);
	}

	return 1;
}

bool svmTrain()
{
	bool bRet = true;

	Mat descMat, labelMat;
	getSampleFeature(mVecSamples, mVecLabels, 36, descMat, labelMat);

	
	bRet = svm->train(descMat, ml::SampleTypes::ROW_SAMPLE, labelMat); //mSVM.train(descMat, labelMat, Mat(), Mat(), mParams);

	svm->save(mStrModelName.c_str());

	return bRet;
}

void LoadXML()
{
	if ( !mIsInit)
	{
		InitSVM();
	}
	svm->load<SVM>(mStrModelName.c_str());
}

bool classifyFlod()
{
	Mat hogMat;
	Mat input_pic = imread("D:\\WRX\\X-ray\\1030\\test1.bmp", 0);
	/*transpose(input_pic, input_pic);
	flip(input_pic, input_pic, 1);
	FindFlodArea(input_pic, hogMat);*/
	GetHOGFeature(input_pic, hogMat);

	return 	svm->predict(hogMat);
}



void FindThodePoints()
{

	/*for (int i = 0; i < g_srcThode.cols; i++)
	{
		for (int j = 0; j < g_srcThode.rows / 2; j++)
		{
			g_srcThode.at<uchar>(j,i) = 255;
		}
	}*/

	cout << "进入查找阳极点" << endl;

	Mat input = g_srcThode.clone();
	Mat input_sobelx, input_scharr;
	GaussianBlur(input, input, Size(3, 3), 0, 0);
	Sobel(input, input_sobelx, -1, 1, 0, 3, 1, 1, BORDER_DEFAULT);
	medianBlur(input_sobelx, input_sobelx, 3);
	Scharr(input, input_scharr, -1, 1, 0, 3, 0, BORDER_DEFAULT);
	g_dFactor = 0.01;
	int start_time = clock();
	double minDistance1 = 21;
	int blocksize = 3, gardensize = 3;
	bool useHarries = false;
	double k = 0.04;
	int _iLayerNum = 10;
	//Mat input_mean = input_sobelx.clone();
	//for (int i = 0; i < input_mean.rows; i++)
	//{
	//	for (int j = 0; j < input.cols ; j++)
	//	{
	//		input_mean.at<uchar>(i, j) = (input_scharr.at<uchar>(i, j) * 0.2 + input_sobelx.at<uchar>(i, j) * 0.8);
	//	}
	//}

	//imshow("input_mean", input_mean);
	goodFeaturesToTrack(input_sobelx, thode, _iLayerNum + 5, g_dFactor, minDistance1, Mat(), blocksize, useHarries, k);
	FixThodePoints();
	ScreenTomsa();
	ScreenThode();
	//使用特特征点检测,补全点集
	//vector<KeyPoint> kpts;
	//Ptr<FastFeatureDetector> fast_detect = FastFeatureDetector::create(30);
	//fast_detect->detect(input_sobelx, kpts);
	////cv::drawKeypoints(srcPic, kpts, srcPic, cv::Scalar::all(0), cv::DrawMatchesFlags::DRAW_OVER_OUTIMG);
	//for (vector<KeyPoint>::iterator it = kpts.begin(); it != kpts.end(); it++)
	//{
	//	thode.push_back(it->pt);
	//}
	//FixThodePoints();
	////再一次清理点集
	///*ScreenThode();
	//ScreenTomsa();*/
	////FixThodePoints();
	////使用高通滤波再计算一次
	//Mat kernel = (Mat_<char>(3, 3) << 0, -1, 0, -1, 5, -1, 0, -1, 0);
	//Mat resultPic;
	//filter2D(input_scharr, resultPic, -1, kernel);
	//convertScaleAbs(resultPic, resultPic);
	//goodFeaturesToTrack(resultPic, thode2, _iLayerNum + 5, g_dFactor, minDistance1, Mat(), blocksize, useHarries, k);

	//for (int i = 0; i < thode2.size(); i++)
	//{
	//	bool flag = true;
	//	for (int j = 0; j < thode.size(); j++)
	//	{
	//		if (abs(thode[j].x - thode2[i].x) <= 15)
	//		{
	//			flag = false;
	//			break;
	//		}
	//	}

	//	if (flag)
	//	{
	//		thode.push_back(thode2[i]);
	//	}

	//}
	//FixThodePoints();
	//ScreenMinBorderPoint();
	// 特征点点修正end
	/*ScreenTomsa();
	ScreenThode();*/
	//FixThodePoints();

	int radius = 3;
	for (int i = 0; i < thode.size(); i++)
	{
		if (thode[i].x == 0 && thode[i].y == 0)
		{
			continue;
		}
		circle(g_srcThode, thode[i], radius, Scalar(0, 0, 255), FILLED);
	}

	int end_time = clock();
	cout << "Tomas找点完成,用时:" << end_time - start_time << endl;

	//最后对点集进行排序
	SortThodePoints();
	imshow("dst", g_srcThode);
}


void TermTest()
{
	cout << "进入查找阳极点" << endl;
	vector<Point2f>counters;
	Mat input = g_srcThode.clone();
	Mat input_sobelx, input_scharr;
	GaussianBlur(input, input, Size(3, 3), 0, 0);
	Sobel(input, input_sobelx, -1, 1, 0, 3, 1, 1, BORDER_DEFAULT);
	medianBlur(input_sobelx, input_sobelx, 3);

	g_dFactor = 0.01;
	int start_time = clock();
	double minDistance1 = 21;
	int blocksize = 3, gardensize = 3;
	bool useHarries = false;
	double k = 0.04;
	int _iLayerNum = 10;
	goodFeaturesToTrack(input_sobelx, thode, _iLayerNum + 5, g_dFactor, minDistance1, Mat(), blocksize, useHarries, k);

	for (int i = 0; i < thode.size(); i++)
	{
		cout <<"(x,y)" <<  thode[i].x  << "," << thode[i].y << endl;
	}


	TermCriteria critetia = TermCriteria(TermCriteria::EPS + TermCriteria::COUNT, 40, 0.001);
	Size winSize = Size(5, 5);
	Size zeroSize = Size(-1, -1);
	cornerSubPix(input_sobelx, thode, winSize, zeroSize, critetia);

	cout << "subpix" << endl;
	for (int i = 0; i < thode.size(); i++)
	{
		cout << "(x,y)" << thode[i].x << "," << thode[i].y << endl;
	}

	int radius = 3;
	for (int i = 0; i < thode.size(); i++)
	{
		if (thode[i].x == 0 && thode[i].y == 0)
		{
			continue;
		}
		circle(g_srcThode, thode[i], radius, Scalar(0, 0, 255), FILLED);
	}

	imshow("dst", g_srcThode);

	//最后对点集进行排序
	//SortThodePoints();
	
}

void FindCathodePoints()
{
	cout << "进入找阴极点函数:" << endl;
	Mat input = g_srcCathode;
	Mat output, output1;
	GaussianBlur(input, output, Size(3, 3), 0, 0);
	Canny(g_srcCathode, output1, 20, 50, 3, true);

	imshow("out", output1);
	//剔除Y轴轮廓的干扰
	for (int i = 0; i < output1.cols; i++)
	{
		for (int j = 0; j < output1.rows-1; j++)
		{
			Scalar pos = output1.at<uchar>(j, i);
			Scalar pos1 = output1.at<uchar>(j+1, i);
			if ( pos[0] == 255 && pos1[0] == 255)
			{
				output1.at<uchar>(j, i) = uchar(0);
			}
		}
	}
	imshow("out1", output1);

	output1.at<uchar>(1, 1) = uchar(0);

	// 沿着X轴方向剔除单个点的干扰
	for (int j = 0; j < output1.rows; j++)
	{
		for (int i = 0; i < output1.cols -2 ; i++)
		{
			Scalar pos = output1.at<uchar>(j, i);
			Scalar pos1 = output1.at<uchar>(j, i+1);
			//Scalar pos2 = output1.at<uchar>(j, i+2);
			if (pos[0] == 255 &&  pos1[0] == 0  )
			{
				output1.at<uchar>(j, i) = uchar(0);
			}
			/*if (pos[0] == 255 && (pos1[0] == 0 || pos2[0] == 0))
			{
			output1.at<uchar>(j, i) = uchar(0);
			}*/
		
		}
	}

	imshow("out2", output1);


	// 去掉单个的点
	for (int j = 0; j < output1.rows - 1 ; j++)
	{
		for (int i = 0; i < output1.cols - 2; i++)
		{
			Scalar pos = output1.at<uchar>(j, i);
			Scalar pos1 = output1.at<uchar>(j, i + 1);
			Scalar pos2 = output1.at<uchar>(j + 1, i);
			if (pos[0] == 255 && pos1[0] == 0 && pos2[0] == 0)
			{
				output1.at<uchar>(j, i) = uchar(0);
			}
			/*if (pos[0] == 255 && (pos1[0] == 0 || pos2[0] == 0))
			{
			output1.at<uchar>(j, i) = uchar(0);
			}*/

		}
	}
	imshow("out2", output1);
	vector<Point> interes;
	for (vector<Point2f>::iterator it = thode.begin(); it != thode.end(); it++)
	{
		if (it->x == 0)
		{
			continue;
		}
		for (int i = output1.rows-1; i >0; i--)
		{
			Scalar pos = output1.at<uchar>(i, it->x - 5);
			Scalar pos1 = output1.at<uchar>(i, it->x - 10);
			Scalar pos2 = output1.at<uchar>(i, it->x - 15);
			if (pos[0] == 255 || pos1[0] == 255 || pos2[0] == 255  )
			{
				interes.push_back(Point(it->x, i));
				break;
			}
		}
		
	}

	for (int i = 0; i < interes.size(); i++)
	{
		circle(input, interes[i], 4, Scalar(0, 0, 255), FILLED);
	}

	imshow("src", input);

	//imwrite("D:\\WRX\\X-ray\\test\\3cathodet.bmp", input);

}

void CalculateK()
{
	Mat g1 = g_srcImage1;
	//GaussianBlur(g1, g1, Size(5, 5), 0, 0);
	// (770,600)->(770,750)从上至下找出几个特征点,这几个点是极值点,比较几个极值点,选出最优点
	int gray_data[150] = {0};
	for (int i = 600; i < 750; i++)
	{
		Scalar pos = g1.at<uchar>(i, 770);
		gray_data[i - 600] = pos[0];
	}
	
	vector<int>minValue;

	int index[150] = {0};	//记录极小值
	for (int i = 1; i < 149; i++)
	{
		if (gray_data[i] <= gray_data[i - 1] && gray_data[i] <= gray_data[i + 1])
		{
			//index[i] = 1;
			minValue.push_back(gray_data[i]);
		}
	}

	//去掉影响平均值的极值
	sort(minValue.begin(), minValue.end());
	for (vector<int>::iterator i = minValue.begin(); i != minValue.end(); i++)
	{	
		cout << *i << endl;
	}

	// 去掉头尾的部分数据,占总体1/3就行,计算平均值
	int sum = 0, meanValue = 0, num = 0;
	int deletenum = minValue.size() / 6;
	for (int i = deletenum - 1; i < minValue.size() - 2 * deletenum; i++)
	{
		sum += minValue[i];
		num++;
	}

	meanValue = sum / num;
	

	Mat src = g_srcImage1(Rect(785, 730, 35, 50));
	Mat grad_x, abs_grad_x, grad_y, abs_grad_y, dst;
	Mat output;
	//Sobel(src, grad_x, CV_16S, 1, 0, 3, 1, 1, BORDER_DEFAULT);
	//convertScaleAbs(grad_x, abs_grad_x);
	//imshow("X方向Sobel", abs_grad_x);

	////【4】求Y方向梯度  
	/*Sobel(src, grad_y, CV_16S, 0, 1, 3, 1, 1, BORDER_DEFAULT);
	convertScaleAbs(grad_y, abs_grad_y);
	imshow("Y方向Sobel", abs_grad_y);*/

	////【5】合并梯度
	//addWeighted(abs_grad_x, 0.5, abs_grad_y, 0.5, 0, dst);
	//imshow("整体方向Sobel", dst);

	threshold(src, src, 10, 255, THRESH_BINARY);

	vector<Vec4i> lines;//存储直线数据
	HoughLinesP(src, lines, 5, CV_PI / 180.0, 10, 30, 0);
	for (size_t i = 0; i < lines.size(); i++)
	{
		Vec4i l = lines[i];
		if ( abs(l[1] - l[3]) <= 5)
		{
			line(src, Point(l[0], l[1]), Point(l[2], l[3]), Scalar(186, 88, 255), 1, LINE_AA);
		}
		
	}
	imshow("dst1", src);
}

void on_Trackbar2(int, void*) // gamma 的回调函数
{
	//建立查找表
	g_dFactor = (double)g_dAlphaValue / 50;
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

	CalculateK();
	imshow("dst", g_dstImage);
}
void StrongPic();

void FilterPic();

void SplitPic();

void FilterThode()
{
	Mat sobelThode;
	Mat input = g_srcThode.clone();
	GaussianBlur(input, input, Size(3, 3), 0, 0);
	Sobel(input, sobelThode, -1, 1, 0, 3, 1, 1, BORDER_DEFAULT);
	convertScaleAbs(sobelThode, sobelThode);
	//imshow("X方向Sobel", sobelThode);
	
	

	for (int i = 0; i < sobelThode.cols; i++)
	{
		for (int j = 0; j < sobelThode.rows; j++)
		{
			Scalar pos = sobelThode.at<uchar>(j, i);
			if (pos[0] < 20)
			{
				sobelThode.at<uchar>(j, i) = 0;
			}
		}
	}

	Mat med_image = sobelThode.clone();
	medianBlur(med_image, med_image, 5);

	for (int i = 0; i < g_srcThode.cols; i++)
	{
		for (int j = 0; j < g_srcThode.rows; j++)
		{
			Scalar pos = med_image.at<uchar>(j, i);
			if ( pos [0] > 0)
			{
				thode.push_back(Point(i, j));
				break;
			}
		}
	}

	// 先按照X轴大小给点排序
	for (int i = 0; i < thode.size(); i++)
	{
		for (int j = 0; j < thode.size() - i - 1; j++)
		{
			Point temp = thode[j];
			if ( thode[j].x > thode[j+1].x)
			{
				thode[j] = thode[j + 1];
				thode[j + 1] = thode[j];
			}
		}
	}

	// 首先要排除离散的点
	for (int i = 0; i < thode.size(); i++)
	{
		cout << "x" << thode[i].x << ",y" << thode[i].y << endl;
	}

	// 把点区间找出来
	vector<int> border;
	for (int i = 0; i < thode.size() - 1; i++)
	{
		if ( thode[i+1].x - thode[i].x > 1 )
		{
			border.push_back(thode[i].x);
			cout << thode[i].x << endl;
		}
		else if (i == thode.size() - 2)
		{
			border.push_back(thode[i + 1].x);
			cout << thode[i + 1].x << endl;
		}
	}

	vector<Point2f> interesPos;

	for (int i = 0; i < border.size(); i++)
	{
		double meanValue = 0, sumValue = 0, nums = 0, posy = 0, posx = 0, sumValuex = 0;
		for (int j = 0; j < thode.size() - 1; j++)
		{
			if ( thode[j].x == 0)
			{
				continue;
			}
			if ( i == 0 && thode[j].x <= border[0])
			{
				nums++;
				sumValue += thode[j].y;
				posy = meanValue;
				sumValuex += thode[j].x;
				posx = sumValuex / nums;
				meanValue = sumValue / nums;

				if (abs(thode[j].y - meanValue) > 5)
				{
					thode[j].x = 0;
					thode[j].y = 0;
				}
			}
			else if (thode[j].x <= border[i] && thode[j].x > border[i - 1])
			{
				nums++;
				sumValue += thode[j].y;
				posy = meanValue;
				sumValuex += thode[j].x;
				posx = sumValuex / nums;
				meanValue = sumValue / nums;

				if (abs(thode[j].y - meanValue) > 5)
				{
					thode[j].x = 0;
					thode[j].y = 0;
				}
			}
		}
		
		interesPos.push_back(Point(posx, posy));
		
	}

	for (int i = 0; i < interesPos.size(); i++)
	{
		cout << "x" << interesPos[i].x << ",y" << interesPos[i].y << endl;
		circle(g_srcThode, Point(interesPos[i].x, interesPos[i].y), 2, Scalar(0, 0, 255), FILLED);
	}

	imshow("dst", g_srcThode);
}


void HighFilter()
{
	Mat med_image = g_srcCathode.clone();
	medianBlur(med_image, med_image,5);
	imshow("Log", med_image);
}

void FindCanny()
{
	Mat input = g_srcImage2.clone();
	Mat input_sobelx;
	GaussianBlur(input, input, Size(3, 3), 0, 0);
	Sobel(input, input_sobelx, -1, 1, 0, 3, 1, 1, BORDER_DEFAULT);
	medianBlur(input_sobelx, input_sobelx, 3);
	imshow("input_sobelx", input_sobelx);
	Mat input_thode;
	GaussianBlur(input_sobelx, input_thode, Size(3, 3), 0, 0);
	Mat kern = getStructuringElement(MORPH_ELLIPSE, Size(3, 3));
	erode(input_thode, input_thode, kern);
	
	Canny(input_thode, input_thode, 20, 55);
	

	for (int i = 0; i < thode.size(); i++)
	{
		circle(g_srcImage2, thode[i], 3, Scalar(0, 0, 255),FILLED);
	}

	for (int i = 0; i < cathode.size(); i++)
	{
		circle(g_srcImage2, cathode[i], 3, Scalar(0, 0, 255), FILLED);
	}

	for (int j = 1; j < 6; j++)
	{
		Point2f pos_begin = thode[thode.size() - j ];
		Point2f pos_end = cathode[cathode.size() - j];

		int posx = pos_begin.x;
		double k = 0, angle = 0;
		for (int i = pos_begin.y; i < pos_end.y; i++)
		{
			Scalar pos = input_sobelx.at<uchar>(i, posx);
			Scalar pos1 = input_sobelx.at<uchar>(i, posx + 1);
			Scalar pos2 = input_sobelx.at<uchar>(i, posx - 1);
			cout << pos[0] << endl;

			int maxValue = max(max(pos[0], pos1[0]), pos2[0]);

			if (maxValue == pos[0])
			{
				continue;
			}
			else if (maxValue == pos2[0])
			{
				cout << "x:" << posx << "y:" << i << endl;
				posx = posx - 1;
				circle(g_srcImage2, Point(posx, i), 2, Scalar(0, 0, 255), FILLED);
				k = abs(posx - pos_begin.x) / abs(i - pos_begin.y);
				angle = atan(k) * 180.0 / 3.1415926;
				cout << "斜率1" << k << "角度" << angle  << endl;
			}
			else if (maxValue == pos1[0])
			{
				cout << "x:" << posx << "y:" << i << endl;
				posx = posx + 1;
				circle(g_srcImage2, Point(posx, i), 2, Scalar(0, 0, 255), FILLED);
				k = abs(posx - pos_begin.x) / abs(i - pos_begin.y);
				angle = atan(k) *180.0 / 3.1415926 ;
				cout << "斜率2" << k << "角度" << angle << endl;
			}

		}
	}

	/*Point2f pos_begin = thode.back();
	Point2f pos_end = cathode.back();

	int posx = pos_begin.x;
	for (int i = pos_begin.y; i < pos_end.y; i++)
	{
		Scalar pos = input_sobelx.at<uchar>(i,posx);
		Scalar pos1 = input_sobelx.at<uchar>(i, posx + 1);
		Scalar pos2 = input_sobelx.at<uchar>(i, posx - 1);
		cout << pos[0] << endl;
		 
		int maxValue =  max( max(pos[0], pos1[0]), pos2[0]);

		if (maxValue == pos[0] )
		{
			continue;
		}
		else if (maxValue == pos2[0])
		{
			cout << "x:" << posx << "y:" << i << endl;
			posx = posx - 1;
			circle(g_srcImage2, Point(posx, i), 2, Scalar(0, 0, 255), FILLED);
		}
		else if (maxValue == pos1[0])
		{
			cout << "x:" << posx << "y:" << i << endl;
			posx = posx + 1;
			circle(g_srcImage2, Point(posx, i), 2, Scalar(0, 0, 255), FILLED);
		}

	}*/

	imshow("g_srcImage2", g_srcImage2);
}

void FindThodeBorder()
{
	Mat input = g_srcThode.clone();
	Mat input_sobelx;
	GaussianBlur(input, input, Size(3, 3),0,0);
	Sobel(input, input_sobelx, -1, 1, 0, 3, 1, 1, BORDER_DEFAULT);
	medianBlur(input_sobelx, input_sobelx, 3);
	imshow("input_sobelx", input_sobelx);

	goodFeaturesToTrack(input_sobelx, thode, 15, 0.01, 21, Mat(), 3, false, 0.04);

	vector<Point2f> featurePos1;
	Mat kernel = (Mat_<char>(3, 3) << 0, -1, 0, -1, 5, -1, 0, -1, 0);
	Mat resultPic;
	filter2D(g_srcThode, resultPic, CV_32F, kernel);
	convertScaleAbs(resultPic, resultPic);
	goodFeaturesToTrack(resultPic, featurePos1, 15, 0.01, 21, Mat(), 3, false, 0.04);

	for (int i = 0; i < featurePos1.size(); i++)
	{
		thode.push_back(featurePos1[i]);
	}

	cout << "筛选：" << endl;

	// 冒泡排序,同时合并相邻的点
	for (int i = 0; i < thode.size(); i++)
	{
		for (int j = 0; j < thode.size() - 1 - i; j++)
		{
			Point2f temp = thode[j];
			if (thode[j].x - thode[j + 1].x > 5)
			{
				thode[j] = thode[j + 1];
				thode[j + 1] = temp;
			}
			else if (abs(thode[j].x - thode[j + 1].x) >= 0 && abs(thode[j].x - thode[j + 1].x) <= 15)
			{
				if (thode[j].y > thode[j + 1].y)
				{
					thode[j] = thode[j + 1];
				}
				else
				{
					thode[j + 1] = thode[j];
				}
			}

		}
	}


	for (int i = 0; i < thode.size(); i++)
	{
		cout << "x" << thode[i].x << "y" << thode[i].y << endl;
	}
	//去掉数据相同的点
	for (vector<Point2f>::iterator it = thode.begin(); it+1 != thode.end();)
	{
		if ( abs(it->x - (it+1)->x) <= 15  )
		{
			it = thode.erase(it);
		}
		else
		{
			it++;
		}
	}

	Mat input_thode;
	GaussianBlur(input_sobelx, input_thode, Size(3, 3), 0, 0);
	threshold(input_thode, input_thode, 30, 255, THRESH_BINARY);

	//利用阈值找到最小的边界位置，用来排除超过最小边界线的点
	int minBorder = input_thode.cols;
	for (int i = 0; i < input_thode.rows; i++)
	{
		for (int j = 0; j < input_thode.cols; j++)
		{
			Scalar pos = input_thode.at<uchar>(i, j);
			if ( pos[0] == 255 )
			{
				if (  minBorder  > j )
				{
					minBorder = j;
				}
			}
		}
	}


	//排除最小边界以外的点
	for (vector<Point2f>::iterator it = thode.begin(); it != thode.end();)
	{
		if ( it->x < minBorder - 5)
		{
			it = thode.erase(it);
		}
		else
		{
			it++;
		}
	}



	for (int i = 0; i < thode.size(); i++)
	{
		circle(input, thode[i], 4, Scalar(0, 0, 0), FILLED);
		cout << "x" << thode[i].x << "y" << thode[i].y << endl;
	}
	imshow("input", input);
	
}

// 精准找阴极区域
void FindCathodeArea()
{
	//imshow("src", g_srcCathode);
	Mat sobel_x;

	Mat grad_x, abs_grad_x, grad_y, abs_grad_y;
	GaussianBlur(g_srcCathode, g_srcCathode, Size(3, 3), 0, 0);
	Scharr(g_srcCathode, grad_x, -1, 1, 0, 1, 0, BORDER_DEFAULT);
	Sobel(g_srcCathode, sobel_x, -1, 1, 0, 3, 1, 1, BORDER_DEFAULT);
	convertScaleAbs(sobel_x, sobel_x);
	convertScaleAbs(grad_x, grad_x);
	//imshow("Sobel1", sobel_x);
	medianBlur(sobel_x, sobel_x, 5);
	medianBlur(sobel_x, sobel_x, 5);
	for (int i = 0; i < sobel_x.cols; i++)
	{
		for (int j = 0; j < sobel_x.rows; j++)
		{
			if (sobel_x.at<uchar>(j, i) <= 2)
			{
				grad_x.at<uchar>(j, i) = 0;
			}
		}
	}


	imshow("【效果图】X方向Scharr", grad_x);
	imshow("【效果图】X方向Sobel", sobel_x);
	for (int i = 0; i < thode.size() - 1; i++)
	{
		Mat dst;
		if ( i == 0 )
		{
			dst = grad_x(Rect(thode[i].x + 15, 0, thode[i + 1].x - thode[i].x - 20, grad_x.rows));
		}
		else if (i < thode.size() - 3)
		{
			dst = grad_x(Rect(thode[i].x + 5, 0, thode[i + 1].x - thode[i].x - 10, grad_x.rows));
		}
		else
		{
			dst = grad_x(Rect(thode[i].x + 5, 0, thode[i + 1].x - thode[i].x - 5, grad_x.rows));
		}
		Scalar meanValue = 0.0, staValue = 0.0;
		meanStdDev(dst, meanValue, staValue);
		threshold(dst, dst, (meanValue[0] + staValue[0]) / 2, 255, THRESH_BINARY);

	
		int rightPos = 0, leftPos = 0;
		if ( i < thode.size() - 3 )
		{
			for (int j = 0; j < dst.cols - 1; j++)
			{ 
				Scalar pos = dst.at<uchar>(dst.rows - 5, j);
				if (pos[0] == 255)
				{
					leftPos = j;
					break;
				}
			}

			if (leftPos < dst.cols / 5)
			{
				leftPos = dst.cols / 5;
			}
		}
		else
		{
			for (int j = 0; j < dst.cols - 1; j++)
			{
				Scalar pos = dst.at<uchar>(dst.rows - 1, j);
				Scalar pos1 = dst.at<uchar>(dst.rows - 1, j + 1);
				if (pos[0] == 0 && pos1[0] == 255 )
				{
					leftPos = j;
					break;
				}
			}

		}
		
		for (int j = dst.cols - 1; j > 1; j--)
		{
			Scalar pos = dst.at<uchar>(dst.rows - 1, j);
			if (pos[0] == 255)
			{
				rightPos = j;
				break;
			}
		}

		if (rightPos < leftPos)
		{
			rightPos = dst.cols - leftPos;
		}

		if (leftPos > rightPos)
		{
			int temp = leftPos;
			leftPos = rightPos;
			rightPos = temp;
		}

		// 先清除边界周边的干扰
		for (int j = 0; j < dst.cols; j++)
		{
			for (int k = 0; k < dst.rows; k++)
			{
				if ( j > rightPos || j < leftPos)
				{
					dst.at<uchar>(k, j) = 0;
				}
			}
		}


		int posy = 0;
		int rate = 50;
		if (i < thode.size() - 2)
		{
			while ( posy ==0 )
			{
				for (int j = 0; j < dst.rows; j++)
				{
					int nums = 0;
					for (int k = leftPos; k <= rightPos; k++)
					{
						Scalar pos = dst.at<uchar>(j, k);
						if (pos[0] == 255)
						{
							nums++;
						}

						if (nums >= rate * (rightPos - leftPos + 1) / 100)
						{
							posy = j;
							nums = 0;
							break;
						}
					}

					if (posy != 0)
					{
						break;
					}
				}

				if ( posy == 0 )
				{
					rate = rate - 10;
				}
			}

		}
		else
		{
			int k = 0;
			for (int j = 0; j < dst.rows - 1; j++)
			{
				Scalar pos = dst.at<uchar>(j, (rightPos + leftPos)/2 );
				Scalar pos1 = dst.at<uchar>(j + 1 , (rightPos + leftPos) / 2);
				if ( pos1[0] - pos[0] >= k)
				{
					posy = j;
					k = pos1[0] - pos[0];
				}
			}
		}

		line(dst, Point(leftPos, dst.rows - 1), Point(leftPos, posy), Scalar(255, 255, 255));
		line(dst, Point(rightPos, dst.rows - 1), Point(rightPos, posy), Scalar(255, 255, 255));
		line(dst, Point(leftPos, posy), Point(rightPos, posy), Scalar(255, 255, 255));

		if (i == 0)
		{
			cathode.push_back(Point2f(thode[i].x + 15 + (leftPos + rightPos) / 2, g_srcThode.rows + posy));
		}
		else
		{
			cathode.push_back(Point2f(thode[i].x + (leftPos + rightPos) / 2, g_srcThode.rows + posy));
		}

		
		for (int i = 0; i < dst.cols; i++)
		{
			for (int j = 0; j < dst.rows; j++)
			{
				if (i < leftPos || i > rightPos)
				{
					dst.at<uchar>(j, i) = 0;
				}
				else if (j < posy)
				{
					dst.at<uchar>(j, i) = 0;
				}
				else
				{
					dst.at<uchar>(j, i) = 255;
				}
			}
		}
	}


	for (int j = 0; j < grad_x.rows; j++)
	{
		Scalar pos = grad_x.at<uchar>(j, thode[thode.size() - 1].x + 12 );
		if ( pos[0] >= 55 )
		{
			cathode.push_back(Point2f(thode[thode.size() - 1].x + 12, g_srcThode.rows + j));
			break;
		}
	}

	/*for (int i = 0; i < cathode.size(); i++)
	{
		circle(g_srcCathode, cathode[i], 3, Scalar(0, 0, 255), FILLED);
	}*/
	imshow("dst1", grad_x);
}



int _tmain(int argc, _TCHAR* argv[])
{
	g_srcImage1 = imread("D:\\WRX\\X-ray\\Desktop\\img2\\1.bmp", 0); //3,7,8,9,10   4,5,6,8,9,10
	//flip(g_srcImage1, g_srcImage1, 1);

	//褶皱筛选
	//LoadSampleData("D:\\WRX\\X-ray\\1030");
	//LoadXML();
	//svmTrain();

	//找到电芯区域
	namedWindow("dst", WINDOW_AUTOSIZE);
	GetDetectPoleArea();
	//找到电芯区域end 
	classifyFlod();
	//ClearScharr();
	//通过shiTomas找阳极点
	//TermTest();
	//NewFindThodePoint();
	//FindThodePoints();
	//shiTomas找点end

	//精准找阴极区域
	//FindCathodeArea();
	//////FilterThode();
	//for (int i = 0; i < cathode.size(); i++)
	//{
	//	circle(g_srcImage2, cathode[i], 3, Scalar(0, 0, 255), FILLED);
	//}
	//imshow("dst", g_srcImage2);
	//找轮廓算角度+
	//FindCanny();

	//imwrite("D:\\WRX\\X-ray\\1030\\1-B_dst.bmp", g_srcImage2);
	//通过canny再找阴极点
	//FindCathodePoints();
	//通过canny找阴极点end

	//imshow("dst", g_srcImage1);
	////GetThodeArea();
	//CalculateK();
	//StrongPic();
	//FilterPic();
	//SplitPic();
	//HighFilter();
	//FilterThode();

	waitKey(0);

	return 0;
}


//筛选大法
void StrongPic()
{
	
	//根据梯度最值计算边缘
	//Mat input = g_srcCathode.clone();// (Rect(155, 0, 30, 143));
	//Mat k1_henral = (Mat_<char>(3, 3) << -1, 1, -1, 0, 3, 0, -1, 1, -1);
	//filter2D(input, input, CV_32F, k1_henral);
	//convertScaleAbs(input, input);
	//imshow("input", input);
	//int i1, k = 0;
	//int roi = 5;
	//for (vector<Point2f>::iterator it = thode.begin(); it != thode.end();)
	//{
	//	if (it->x == 0)
	//	{
	//		it = thode.erase(it);
	//	}
	//	else
	//	{
	//		it++;
	//	}
	//}

	//for (int i = 0; i < thode.size(); i++)
	//{
	//	for (int j = 0; j < thode.size() - 1 - i; j++)
	//	{
	//		if (thode[j].x < thode[j + 1].x)
	//		{
	//			Point2f a = thode[j + 1];
	//			thode[j + 1] = thode[j];
	//			thode[j] = a;
	//		}
	//	}
	//}

	//for (int i = 0; i < thode.size(); i++)
	//{
	//	cout << thode[i] << endl;
	//}

	//for (int i = 0; i < thode.size() - 1; i++)
	//{
	//	for (int j = 0; j < input.rows - 1; j++)
	//	{
	//		double gray1 = 0, gray2 = 0;

	//		for (int k = thode[i].x; k > thode[i + 1].x; k--)
	//		{
	//			gray1 += input.at<uchar>(j, k);
	//			gray2 += input.at<uchar>(j + 1, k);
	//		}
	//		gray1 = gray1 / (thode[i].x - thode[i + 1].x);
	//		gray2 = gray2 / (thode[i].x - thode[i + 1].x);

	//		if (gray1 - gray2 > 0)
	//		{
	//			if (abs(gray1 - gray2) > 10)
	//			{
	//				circle(input, Point((thode[i].x + thode[i + 1].x) / 2, j), 2, Scalar(0, 0, 255), 1, 8, false);
	//			}
	//		}
	//	}
	//}

	//cout << k << endl;
	//imwrite("D:\\WRX\\X-ray\\test\\9cathode.bmp", input);
	//imshow("dst", input);

	//Mat grad_y, abs_grad_y;
	//Sobel(g_srcCathode, grad_y, CV_16S, 0, 1, 3, 1, 1, BORDER_DEFAULT);
	//convertScaleAbs(grad_y, abs_grad_y);
	//imshow("Y方向Sobel", abs_grad_y); 

	//根据梯度最值计算边缘

	// 加权增强
	//Scalar mean;  //均值
	//Scalar stddev;  //标准差

	//GaussianBlur(g_srcCathode, g_srcCathode, Size(3, 3), 0, 0);
	//cv::meanStdDev(g_srcCathode, mean, stddev);  //计算均值和标准差 
	//double mean_pxl = mean.val[0];
	//double stddev_pxl = stddev.val[0];

	//for (int i = 0; i < g_srcCathode.cols; i++)
	//{
	//	for (int j = 0; j < g_srcCathode.rows;j++)
	//	{
	//		if (g_srcCathode.at<uchar>(j, i) != 255 && g_srcCathode.at<uchar>(j, i) != 0)
	//		{
	//			Scalar val = round((g_srcCathode.at<uchar>(j, i) - mean_pxl)* 0.2) + g_srcCathode.at<uchar>(j, i);
	//			if (val[0] >= 255 || val[0] <= 0)
	//			{
	//			}
	//			else
	//			{
	//				g_srcCathode.at<uchar>(j, i) = round((g_srcCathode.at<uchar>(j, i) - mean_pxl)* 0.2) + g_srcCathode.at<uchar>(j, i);
	//			}
	//		}
	//	}
	//}
	// 加权增强

	Mat output1;
	GaussianBlur(g_srcCathode, g_srcCathode, Size(3, 3),0,0);
	Canny(g_srcCathode, output1, 10, 50, 3, false);

	imshow("outp1", output1);
	// 沿着Y轴方向剔除干扰
	//for (int i = 0; i < output1.cols; i++)
	//{
	//	for (int j = 0; j < output1.rows - 1; j++)
	//	{
	//		Scalar pos = output1.at<uchar>(j, i);
	//		Scalar pos1 = output1.at<uchar>(j + 1, i);
	//		if (pos[0] == 255 && pos1[0] == 255)
	//		{
	//			output1.at<uchar>(j, i) = uchar(0);
	//		}
	//	}
	//}
	//imshow("out1", output1);

	//// 沿着X轴方向剔除单个点的干扰
	//for (int j = 0; j < output1.rows; j++)
	//{
	//	for (int i = 1; i < output1.cols - 1; i++)
	//	{
	//		Scalar pos = output1.at<uchar>(j, i);
	//		Scalar pos1 = output1.at<uchar>(j, i + 1);
	//		Scalar pos2 = output1.at<uchar>(j, i - 1);
	//		if (pos[0] == 255 && pos1[0] == 0 && pos2[0] == 0)
	//		{
	//			output1.at<uchar>(j, i) = uchar(0);
	//		}
	//	}
	//}

	//imshow("out2", output1);

	imshow("dst", g_srcCathode);

	imwrite("D:\\WRX\\X-ray\\test\\out2.bmp", output1);
	imwrite("D:\\WRX\\X-ray\\test\\src.bmp", g_srcCathode);
	vector<Point2f> interes;

	vector<Point2f> sortThode;

	for (int i = 0; i < thode.size(); i++)
	{
		if (thode[i].x != 0)
		{
			sortThode.push_back(thode[i]);
		}
	}

	// 冒泡排序,X方向最大值-->最小值
	for (int i = 0; i < sortThode.size(); i++)
	{
		for (int j = 0; j < sortThode.size() - 1 - i; j++)
		{
			if (sortThode[j].x < sortThode[j+1].x)
			{
				Point2f a = sortThode[j+1];
				sortThode[j+1] = sortThode[j];
				sortThode[j] = a;
			}
		}
	}

	for (vector<Point2f>::iterator it = sortThode.begin(); it != sortThode.end();)
	{
		if (it->x == 0)
		{
			it = sortThode.erase(it);
		}
		else
		{
			it++;
		}
	}

	for (int i = 0; i < sortThode.size(); i++)
	{
		cout << sortThode[i].x << ":" << sortThode[i].y << endl;
	}

	cout << "找到全部可能的边缘点" << endl;
	for (int i = 0; i < sortThode.size() - 1; i++)
	{
		for (int j = output1.rows / 2; j > 0; j--)
		{
			Scalar pos = output1.at<uchar>(j, sortThode[i].x - 5);
			Scalar pos1 = output1.at<uchar>(j, (sortThode[i].x + sortThode[i + 1].x)/2);
			Scalar pos2 = output1.at<uchar>(j, sortThode[i + 1].x + 5);
			if ( pos1[0] == 255 )
			{
				interes.push_back(Point((sortThode[i].x + sortThode[i + 1].x) / 2, j));
			}
			else if (pos[0] == 255)
			{
				interes.push_back(Point(sortThode[i].x - 5, j));
			}
			else if (pos2[0] == 255)
			{
				interes.push_back(Point(sortThode[i + 1].x + 5, j));
			}
		}
	}

	/*for (int i = 0; i < interes.size(); i++)
	{
		cout << interes[i].x << ":" << interes[i].y << endl;
	}*/

	cout << "筛选干扰点" << endl;

	// 首先要筛选掉那些干扰点而被认为是边缘的点
	for (int i = 0; i < interes.size() - 1; i++)
	{
		int posx = interes[i].x;
		Scalar pos1 = output1.at<uchar>(interes[i].y, posx - 1);
		Scalar pos2 = output1.at<uchar>(interes[i].y, posx + 1);

		//cout << interes[i].x << ":" << interes[i].y << ":" << pos1[0] << ";" << pos2[0] << endl;
		if (pos1[0] != 255 || pos2[0] != 255 )
		{
			interes[i].x = 0;
			interes[i].y = 0;
		}

	}
	

	for (vector<Point2f>::iterator it = interes.begin(); it != interes.end(); )
	{
		if ( it->x == 0)
		{
			it = interes.erase(it);
		}
		else
		{
			it++;
		}
	}


	//for (int i = 0; i < interes.size(); i++)
	//{
	//	cout << "x" << interes[i].x << ",y" << interes[i].y << endl;
	//}
	cout << "筛选干扰点end" << endl;

	cout << "筛选候选点" << endl;
	// 电芯分布是局部单调的,先假设是单调递增,这时图像外一层比内一层Y方向坐标低
	int LeftBorder, RightBorder, keyValue = 0, keyValue1 = 0;
	bool isIncrease = true;//假定为单调增
	vector <Point2f>interes1;
	for (int i = 0; i < sortThode.size() - 1; i++)
	{
		cout << "第" << i << endl;
		isIncrease = true;
		LeftBorder = sortThode[i + 1].x;
		RightBorder = sortThode[i].x;

		vector<Point2f> detectPos;
		for (int i = 0; i < interes.size(); i++)
		{
			if (interes[i].x > LeftBorder && interes[i].x <= RightBorder)
			{
				detectPos.push_back(interes[i]);
			}
		}

		//Y方向排序,为了后面找点做准备
		for (int i = 0; i < detectPos.size(); i++)
		{
			for (int j = 0; j < detectPos.size() - 1 - i; j++)
			{
				if (detectPos[j].y > detectPos[j + 1].y)
				{
					Point2f a = detectPos[j + 1];
					detectPos[j + 1] = detectPos[j];
					detectPos[j] = a;
				}
			}
		}

		// 增加突变点筛选
		for (int j = 0; j < interes1.size(); j++)
		{
			for (vector<Point2f>::iterator it = detectPos.begin(); it != detectPos.end();)
			{
				if ( detectPos.size() == 1)
				{
					break;
				}
				if (it->y == interes1[j].y)
				{
					it = detectPos.erase(it);
				}
				else
				{
					it++;
				}
			}

		}

		// 最外层的相对于0 白色区域肯定是单调增,所以最外层可以直接认为最大值是边缘点
		if ( i == 0 )
		{
			Point2f pos(0,0);
			for (vector<Point2f>::iterator it = detectPos.begin(); it != detectPos.end(); it++)
			{
				if (it->y > keyValue) // 找到Y轴方向最大
				{
					keyValue = it->y;
					pos.x = it->x;
					pos.y = it->y;
				}
			}
			interes1.push_back(pos);
		}
		else
		{
			Point2f pos(0,0);

			for (vector<Point2f>::iterator it = detectPos.begin(); it != detectPos.end(); it++) //先判断单调性
			{

				if (it->y > keyValue) //如果有大于相邻层边缘的那么认为是单调增的
				{
					isIncrease = true;
					break;
				}
				else // 重新找否则该层的相邻单调增的层
				{
					isIncrease = false;
					keyValue1 = keyValue;
				}
			}
			
			if (!isIncrease) // 重新找该层的相邻层
			{
				keyValue = detectPos[detectPos.size()-1].y;
				
				cout << "重新找相邻层之前" << keyValue1 << keyValue << endl;

				for (int i = interes1.size()-1; i >0; i--)
				{
					if (keyValue > interes1[i].y)
					{
						keyValue = interes1[i].y;
						break;
					}
				}

				for (int j = 0; j < interes1.size() - 1; j++)
				{
					if (keyValue < interes1[j].y )
					{
						keyValue1 = interes1[j].y;
						break;
					}
				}

				cout << "重新找相邻层" << keyValue1 << keyValue << endl;
			}


			for (int i = 0; i < detectPos.size(); i++)
			{
				cout << "detect" << detectPos[i].x << ":" << detectPos[i].y << endl;
			}
			// 找出最大的值作为该出的边缘点
			for (vector<Point2f>::iterator it = detectPos.begin(); it != detectPos.end(); it++) //找极值
			{
				if (isIncrease)
				{
					if (it->y >= keyValue)
					{
						keyValue = it->y;
						pos.x = it->x;
						pos.y = it->y;
					}
				}
				else
				{
					if (it->y < keyValue1 && it->y >= keyValue )
					{
						keyValue = it->y;
						pos.x = it->x;
						pos.y = it->y;
					}
				}
			}
			interes1.push_back(pos);
		}
	}

	//cout << "最后确定的边缘点" << endl;
	for (int i = 0; i < interes1.size(); i++)
	{
		cout << interes1[i].x  << ":" << interes1[i].y << endl;
	}

}

//scharr找阴极
void FilterPic()
{
	/*Mat grad_x;
	GaussianBlur(g_srcCathode, g_srcCathode, Size(3, 3), 1, 1);
	Sobel(g_srcCathode, grad_x, -1, 1, 0, 3, 1, 0, BORDER_DEFAULT);
	convertScaleAbs(grad_x, grad_x);
	imshow("src", grad_x);*/
	Mat grad_x, abs_grad_x, grad_y, abs_grad_y;
	GaussianBlur(g_srcCathode, g_srcCathode, Size(3, 3), 0, 0);
	Scharr(g_srcCathode, grad_x, -1, 1, 0, 1, 0, BORDER_DEFAULT);
	convertScaleAbs(grad_x, grad_x);
	imshow("【效果图】X方向Scharr", grad_x);
	

	for (vector<Point2f>::iterator it = thode.begin(); it != thode.end();  )
	{
		if ( it->x == 0)
		{
			it = thode.erase(it);
		}
		else
		{
			it++;
		}
	}

	for (int i = 0; i < thode.size(); i++)
	{
		for (int j = 0; j < thode.size() - i - 1; j++)
		{
			Point2f a = thode[j];
			if ( thode[j + 1].x < thode[j].x )
			{
				thode[j] = thode[j + 1];
				thode[j + 1] = a;
			}
		}
	}

	for (int i = 0; i < thode[0].x; i++)
	{
		for (int j = 0; j < grad_x.rows; j++)
		{
			grad_x.at<uchar>(j, i) = 0;
		}
	}

	for (vector<Point2f>::iterator it = thode.begin(); it != thode.end(); it++)
	{
		for (int j = 0; j < grad_x.rows; j++)
		{
			grad_x.at<uchar>(j, it->x) = 0;
		}
	}


	//膨胀腐蚀
	
	//for (int i = 0; i < thode.size() - 1; i++)
	//{
	//	//
	//	Mat dst;
	//
	//
	//	if ( i < thode.size() - 3 )
	//	{
	//		dst = grad_x(Rect(thode[i].x + 10, 0, thode[i + 1].x - thode[i].x - 15, grad_x.rows));
	//	}
	//	else
	//	{
	//		dst = grad_x(Rect(thode[i].x + 5, 0, thode[i + 1].x - thode[i].x - 5 , grad_x.rows));
	//	}
	//	Scalar meanValue = 0.0, staValue = 0.0;
	//	meanStdDev(dst, meanValue, staValue);
	//	threshold(dst, dst, (meanValue[0] + staValue[0])/2, 255, THRESH_BINARY);
	//	/*if (i == 0)
	//	{
	//	Mat kern = getStructuringElement(MORPH_ELLIPSE, Size(3, 3));
	//	dilate(dst, dst, kern);
	//	kern = getStructuringElement(MORPH_ELLIPSE, Size(1, 1));
	//	erode(dst, dst, kern);
	//	}*/
	//	                                                                                     
	//}

	
	for (int i = 0; i < thode.size() - 1; i++)
	{
		Mat dst;
		if (i < thode.size() - 3)
		{
			dst = grad_x(Rect(thode[i].x + 10, 0, thode[i + 1].x - thode[i].x - 15, grad_x.rows));
		}
		else
		{
			dst = grad_x(Rect(thode[i].x + 5, 0, thode[i + 1].x - thode[i].x - 5, grad_x.rows));
		}
		Scalar meanValue = 0.0, staValue = 0.0;
		meanStdDev(dst, meanValue, staValue);
		threshold(dst, dst, (meanValue[0] + staValue[0]) / 2, 255, THRESH_BINARY);

		int rightPos, leftPos;

		for (int j = 0; j < dst.cols; j++)
		{
			Scalar pos = dst.at<uchar>(dst.rows - 1,j);
			if (pos[0] == 255 )
			{
				leftPos = j;
				break;
			}
		}

		for (int j = dst.cols-1; j > 0; j--)
		{
			Scalar pos = dst.at<uchar>(dst.rows - 1, j);
			if (pos[0] == 255 )
			{
				rightPos = j;
				break;
			}
		}
		
		int posy = 0;

		if ( i < thode.size() - 3 )
		{
			for (int j = 0; j < dst.rows; j++)
			{
				int nums = 0;
 				for (int k = leftPos; k < rightPos; k++)
				{
					Scalar pos = dst.at<uchar>(j, k);
					if (pos[0] == 255)
					{
						nums++;
					}

					if (nums >= 40 * (rightPos - leftPos + 1) / 100)
					{
						cout << rightPos << "," << leftPos << ":" << j << endl;
						cout << rightPos - leftPos << endl;
						cout << nums << endl;

						posy = j;
						nums = 0;
						break;
					}
				}

				if (posy != 0)
				{
					break;
				}
			}
		}
		else // 使用梯度计算前2层的posy值
		{
			dst = grad_x(Rect(thode[i].x + 5, 0, thode[i + 1].x - thode[i].x - 5, grad_x.rows));
			int k = 0;
			for (int j = 0; j < dst.rows - 1; j++)
			{
				Scalar pos = dst.at<uchar>(j, dst.cols / 2);
				Scalar pos1 = dst.at<uchar>(j + 1, dst.cols / 2);

				if ( abs(pos1[0] - pos[0]) > k)
				{
					k = abs(pos1[0] - pos[0]);
					posy = j;
				}
			}
		}

		line(dst, Point(leftPos, dst.rows - 1), Point(leftPos, posy), Scalar(255, 255, 255));
		line(dst, Point(rightPos, dst.rows - 1), Point(rightPos, posy), Scalar(255, 255, 255));
		line(dst, Point(leftPos, posy), Point(rightPos, posy), Scalar(255, 255, 255));

		for (int i = 0; i < dst.cols; i++)
		{
			for (int j = 0; j < dst.rows; j++)
			{
				if (i < leftPos || i > rightPos)
				{
					dst.at<uchar>(j, i) = 0;
				}
				else if (j < posy)
				{
					dst.at<uchar>(j, i) = 0;
				}
				else
				{
					dst.at<uchar>(j, i) = 255;
				}
			}
		}
	}

	imshow("dst", grad_x);


	//int layers = thode.size() - 1;
	//for (int i = 0; i < thode.size() - 1; i++)
	//{
	//
	//	int rightPos = thode[i + 1].x;
	//	int leftPos = thode[i].x;
	//	for (int j = rightPos; j >(leftPos + rightPos) / 2; j--)
	//	{
	//		if (i < layers - 2)
	//		{
	//			for (int k = 0; k < grad_x.rows - 1; k++)// 内部3层按照从上往下找,找到一个就认为是边缘点
	//			{
	//				Scalar pos = grad_x.at<uchar>(k, j);
	//				Scalar pos1 = grad_x.at<uchar>(k + 1, j);
	//				if (pos[0] == 0 && pos1[0] == 255)
	//				{
	//					circle(g_srcCathode, Point(j, k + 1), 3, Scalar(0, 0, 255));
	//					break;
	//				}
	//			}
	//		}
	//		else 
	//		{
	//			for (int k = grad_x.rows-1; k > 1; k--)// 最后3层开始按照从下往上找,找到最后一个就认为是边缘点
	//			{
	//				Scalar pos = grad_x.at<uchar>(k, j);
	//				Scalar pos1 = grad_x.at<uchar>(k - 1, j);
	//				if (pos[0] == 255 && pos1[0] == 0)
	//				{
	//					circle(g_srcCathode, Point(j, k), 3, Scalar(0, 0, 255));
	//					break;
	//				}
	//			}
	//		}
	//	}
	//
	//}


	/*double sumValue = 0.0, meanValue =0.0;
	for (int i = 0; i < grad_x.cols; i++)
	{
		sumValue = 0;
		for (int j = 0; j < grad_x.rows; j++)
		{
			Scalar pos = grad_x.at<uchar>(j, i);
			sumValue += pos[0];
		}

		meanValue = sumValue / grad_x.rows;


		if (meanValue <= 25)
		{
			for (int j = 0; j < grad_x.rows; j++)
			{
				grad_x.at<uchar>(j, i) = 0;
			}
		}
	}*/
	//imshow("dst", grad_x);


	/*Mat cannyX;
	Canny(grad_x, cannyX, 30, 50);
	imshow("cannyX", cannyX);*/
	//GaussianBlur(grad_x, grad_x, Size(3, 3), 0, 0);

	/*Mat grad_y, abs_grad_y;
	Sobel(g_srcCathode, grad_y, CV_16S, 0, 1, 3, 1, 1, BORDER_DEFAULT);
	convertScaleAbs(grad_y, abs_grad_y);
	imshow("Y方向Sobel", abs_grad_y);*/


	/*Mat openPic;
	Mat kernel = getStructuringElement(MORPH_RECT, Size(3, 3), Point(-1, -1));
	morphologyEx(abs_grad_y, openPic, MORPH_TOPHAT, kernel);
	imshow("open", openPic);

	GaussianBlur(openPic, openPic, Size(3, 3), 1, 1);
	Mat cannyY;
	Canny(openPic, cannyY, 30, 50);
	imshow("cannyY", cannyY);
	*/

	/*imshow("X方向Sobel", grad_x);
	imshow("dst", g_srcCathode);*/

	//threshold(cannyY, cannyY, 30, 50,CV_THRESH_BINARY_INV);
	//threshold(cannyX, cannyX, 30, 50, CV_THRESH_BINARY_INV);

	//imshow("cannyY1", cannyY);
	//imshow("cannyX1", cannyX);
	//imshow("src", g_srcCathode);

	//Mat grad_x1 = grad_x.clone();

	//double sumValue = 0.0, meanValue =0.0;
	//for (int i = 0; i < grad_x1.cols; i++)
	//{
	//	sumValue = 0;
	//	for (int j = 0; j < grad_x1.rows; j++)
	//	{
	//		Scalar pos = grad_x1.at<uchar>(j, i);
	//		sumValue += pos[0];
	//	}

	//	meanValue = sumValue / grad_x1.rows;


	//	if (meanValue <= 25)
	//	{
	//		for (int j = 0; j < grad_x1.rows; j++)
	//		{
	//			grad_x1.at<uchar>(j, i) = 0;
	//		}
	//	}
	//}


	//sumValue = 0.0, meanValue = 0.0;
	//for (int i = 0; i < openPic.rows; i++)
	//{
	//	sumValue = 0;
	//	for (int j = 0; j < openPic.cols; j++)
	//	{
	//		Scalar pos = openPic.at<uchar>(i,j);
	//		sumValue += pos[0];
	//	}

	//	meanValue = sumValue / openPic.rows;


	//	if (meanValue <= 20)
	//	{
	//		for (int j = 0; j < openPic.cols; j++)
	//		{
	//			openPic.at<uchar>(i,j) = 0;
	//		}
	//	}
	//}


	//Mat dst;
	//addWeighted(grad_x1, 0.3, openPic, 0.7, 0, dst);
	//imshow("整体方向Sobel", dst);

	//imshow("grad_x1", grad_x1);
	//imshow("grad_y1", openPic);


	//Mat input = imread("D:\\WRX\\X-ray\\test\\3cathode.bmp", 0);
	//Mat h1_kernel = (Mat_<char>(3, 3) << 0, -1, 0, -1, 4, -1, 0, -1, 0);
	////Mat h1_kernel = (Mat_<char>(5, 5) << 3, 0, 0, 0, 0, 1, 0, 0, 0, 0, -1, -1, -1, 0, 0, -1, 0, 0, 0, 0, 1, -1, 0, -1, 1);
	////Mat h1_kernel = (Mat_<char>(5, 5) << -1, 0, -1, 0, -1, -1, 0, -1, 0, -1, -1, 3, 3, 3, -1, -1, 0, 3, 0, -1, -1, 0, 3, 0, -1);
	//Mat h1_result;
	//filter2D(input, h1_result, -1, h1_kernel);
	//convertScaleAbs(h1_result, h1_result);
	//imshow("h1_result", h1_result);
	
}


void SplitPic()
{
	Mat input = g_srcThode, input2 = g_srcCathode.clone();

	//先从阳极的部分取灰度的均值

	vector<Point2f> sortThode;
	vector<double> grayLevel;
	vector<Mat> splitRect;
	for (int i = 0; i < thode.size(); i++)
	{
		if ( thode[i].x != 0)
		{
			sortThode.push_back(thode[i]);
		}
	}

	for (int i = 0; i < sortThode.size(); i++)
	{
		for (int j = 0; j < sortThode.size() - i - 1; j++)
		{
			if ( sortThode[ j + 1 ].x > sortThode[j].x)
			{
				Point2f temp = sortThode[j + 1];
				sortThode[j + 1] = sortThode[j];
				sortThode[j] = temp;
			}
		}
	}

	for (int i = 0; i < sortThode.size() - 1; i++)
	{
		int rectWidth = sortThode[i].x - sortThode[i + 1].x;
		int rectHeight = sortThode[i].y > sortThode[i + 1].y ? sortThode[i].y : sortThode[i + 1].y;

		Scalar meanValue, stdValue;
		meanStdDev(input(Rect(sortThode[i + 1].x, sortThode[i + 1].y, rectWidth, input.rows - rectHeight)), meanValue, stdValue);
		splitRect.push_back(input2(Rect(sortThode[i + 1].x, 0, rectWidth, rectHeight)));
		grayLevel.push_back(meanValue[0]);
	}

	for (int i = 0; i < grayLevel.size(); i++)
	{
		cout << grayLevel[i] << endl;
	}

	for (int i = 0; i < splitRect.size(); i++)
	{
		int T = 0;
		
		if (i <= splitRect.size() / 3)
		{
			T  = AtsoThreshold(splitRect[i]);
		}
		else if (i <= 2 * splitRect.size() / 3)
		{
			T = std::min<int>(AtsoThreshold(splitRect[i]), 80);
		}
		else
		{
			T = std::min<int>(AtsoThreshold(splitRect[i]), 50);
		}

		threshold(splitRect[i], splitRect[i], T, 255, CV_THRESH_BINARY_INV);
		
	}

	imshow("out", input2);
	imshow("dst", input);
	
}

//ORB 找寻特征点
void ORBTest()
{
	//ORB 找寻特征点
	//Mat srcPic = imread("D:\\WRX\\X-ray\\1030\\1.bmp");
	//Mat dstPic = srcPic.clone();
	//auto orb_detector = ORB::create(10);
	//vector<KeyPoint> kpts;
	//// 开启ORB检查
	//orb_detector->detect(srcPic, kpts);
	//// 绘制关键点
	//drawKeypoints(srcPic, kpts, dstPic, Scalar(0, 0, 255), DrawMatchesFlags::DEFAULT);
	//imshow("src", dstPic);
	//ORB 找寻特征点
}

//角点检测
void AnglePointTest()
{
	//角点检测
	//Mat srcPic = imread("D:\\WRX\\X-ray\\1030\\5.bmp");
	//Mat src;
	//cvtColor(srcPic, src, CV_RGB2GRAY);
	//Mat dstPic = Mat::zeros(srcPic.size(), CV_32FC1);
	//Mat norm_dst, normScaleDst;
	//cornerHarris(src, dstPic, 3, 3, 0.01);
	////threshold(dstPic, dstPic, 0.0001, 255, THRESH_BINARY);
	//normalize(dstPic, norm_dst, 0, 255, NORM_MINMAX, CV_32FC1, Mat());
	//convertScaleAbs(norm_dst, normScaleDst);
	//imshow("dst1", normScaleDst);
	//角点检查end

	//快速角点检查
	/*Mat srcPic = imread("D:\\WRX\\X-ray\\1030-1\\4.bmp");
	vector<KeyPoint> kpts;
	Ptr<FastFeatureDetector> fast_detect = FastFeatureDetector::create(20);
	fast_detect->detect(srcPic, kpts);
	cv::drawKeypoints(srcPic, kpts, srcPic, cv::Scalar::all(0), cv::DrawMatchesFlags::DRAW_OVER_OUTIMG);
	imshow("dst1", srcPic);*/
	//快速角点检查 end
}


//Laplacian高通滤波算子
void Laplacian()
{
	//Laplacian高通滤波算子
	/*Mat input = imread("D:\\WRX\\X-ray\\1030\\8.bmp", 0);
	Mat h1_kernel = (Mat_<char>(3, 3) << 2, 0, 0, 0, -1, 0, 0, 0, -1);
	Mat h2_kernel = (Mat_<char>(3, 3) << 0, -1, 0, -1, 5, -1, 0, -1, 0);
	Mat h3_kernel = (Mat_<char>(5, 5) << -1,-1, -1, -1, -1, -1, 1, 2, 1, -1, -1, 2, 4, 2, -1, 1, 2, 1, -1, -1, -1, -1, -1, -1, -1);
	Mat h1_result, h2_result, h3_result;
	filter2D(input, h1_result, CV_32F, h1_kernel);
	filter2D(input, h2_result, CV_32F, h2_kernel);
	filter2D(input, h3_result, CV_32F, h3_kernel);
	convertScaleAbs(h1_result, h1_result);
	convertScaleAbs(h2_result, h2_result);
	convertScaleAbs(h3_result, h3_result);
	imshow("h1_result", h1_result);
	imshow("h2_result", h2_result);
	imshow("h3_result", h3_result);*/
	/*GaussianBlur(h2_result, h2_result, Size(3, 3), 0, 0);
	Canny(h2_result, g_dstImage, 25, 50, 3, true);
	imshow("dst1", g_dstImage);*/
	//threshold(imgThreshold, imgThreshold, 230, 255, THRESH_BINARY); //CV_THRESH_OTSU
}

//Unshrpen Mask算法
void UpshrpenMask()
{
	//Unshrpen Mask算法
	/*Mat input = imread("D:\\WRX\\X-ray\\1030-1\\13.bmp");
	Mat blur, usm;
	GaussianBlur(input, blur, Size(0, 0), 25);
	addWeighted(input, 1.5, blur, -0.5, 0, usm);
	imshow("usm", usm);*/
	//Unshrpen Mask算法
}

//canny算子提取直线和轮廓
void CannyTest()
{
	//canny算子提取直线和轮廓
	//Mat input = imread("D:\\WRX\\X-ray\\1030-1\\1.bmp");
	//Mat output, output1;
	//GaussianBlur(input, output, Size(3, 3), 0, 0);
	////bilateralFilter(input, output, 10, 40, 40);
	//Canny(output, output1, 25, 50, 3, true);
	//imshow("out", output1);
	//int posx = 572;
	//vector<Point> interes;
	//for (int i = 257; i < output1.rows; i++)
	//{
	//	Scalar pos = output1.at<uchar>(i, posx);
	//	if (pos[0] == 255)
	//	{
	//		interes.push_back(Point(posx, i));
	//	}
	//}
	//for (int i = 0; i < interes.size();i++)
	//{
	//	circle(input, interes[i], 4, Scalar(0, 0, 255), FILLED);
	//}
	//imshow("src", input);
	//canny算子提取直线和轮廓
}

//创建拖动条
void UseTrackBar()
{
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
	/*g_dAlphaValue = 15;
	g_srcImage1 = imread("D:\\WRX\\X-ray\\1030\\1.bmp",0);
	namedWindow("dst", WINDOW_AUTOSIZE);
	createTrackbar("gamma", "dst", &g_dAlphaValue, 100, on_Trackbar2);
	on_Trackbar2(g_dAlphaValue, 0);*/
	//gamma变换

}

//高斯函数差分DOG
void UseGaussDog()
{
	//高斯函数差分DOG
	/*Mat srcPic = imread("D:\\WRX\\X-ray\\1030\\7.bmp",0);
	//Mat dstPic, g1, g2, dogImg;
	////两次高斯模糊
	//Mat g_srcImage1 = srcPic(Rect(600,600,330,200));
	//int start = clock();
	//GaussianBlur(g_srcImage1, g1, Size(3, 3), 0, 0);
	//GaussianBlur(g1, g2, Size(3, 3), 0);

	//subtract(g1, g2, dogImg, Mat());   //差分图的灰度值比较小，图比较暗。
	////归一化显示
	//normalize(dogImg, dogImg, 255, 0, NORM_MINMAX);  //归一化，放到0-255显示。

	//std::vector<std::vector<cv::Point>> contours;
	//vector<Vec4i> hireachy;


	//findContours(dogImg, contours, hireachy, CV_RETR_TREE, CV_CHAIN_APPROX_NONE);

	//Mat result_image = Mat::zeros(dogImg.size(), CV_8UC3);

	//Scalar color[] = { Scalar(0, 0, 255), Scalar(0, 255, 0), Scalar(255, 0, 0), Scalar(255, 255, 0), Scalar(255, 0, 255) };

	////循环找出所有符合条件的轮廓
	//Mat pts;
	//for (size_t t = 0; t < contours.size(); t++)
	//{
	//	//条件：过滤掉小的干扰轮廓
	//	Rect rect = boundingRect(contours[t]);
	//	if (rect.width < 5)
	//		continue;
	//	//画出找到的轮廓
	//	drawContours(result_image, contours, static_cast<int>(t), Scalar(255, 255, 255), 1, 8, hireachy);

	//	approxPolyDP(contours[t], pts, 25, true);

	//}
	//7.4 画出每一个点
	/*for (size_t i = 0; i < pts.rows; i++)
	{
	Vec2i pt = pts.at<Vec2i>(i, 0);
	cout << "   " << pt << endl;
	circle(result_image, Point(pt[0], pt[1]), 2, Scalar(0, 255, 0), -1);
	}

	int endt = clock();

	cout << endt - start << endl;

	imshow("src", result_image);*/

	//DOG结束
	//Mat dstPic1, srcPic1, norm_dst, normScaleDst;
	//cornerHarris(srcPic1, dstPic1, 3, 3, 0.01);
	//threshold(dstPic1, dstPic1, 0.0001, 255, THRESH_BINARY);
	//normalize(dstPic1, norm_dst, 0, 255, NORM_MINMAX, CV_32FC1, Mat());
	//convertScaleAbs(norm_dst, normScaleDst);
	//
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
}

//判断图片为纯白/黑图
void JudgeBlackOrWhite()
{
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
	imshow("src", g_srcImage1);

	cv::Point origin;
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
}

//测试AtsoThreshold
void AtsoThreshold()
{
	//uint8 inMinValue, inMaxValue;
	//auto cvArray = Mat(g_srcImage1);
	//auto transposedCvArray = cv::Mat(cvArray.cols, cvArray.rows, CV_8SC1);
	//transpose(cvArray, transposedCvArray);
	//NdArray<uint8> inArray = NdArray<uint8>(transposedCvArray.data, transposedCvArray.rows, transposedCvArray.cols);
	//clip(&inArray, inMinValue, inMaxValue); */

	//测试AtsoThreshold
	/*获取图像
	//g_srcImage1 = imread("D:\\WRX\\X-ray\\1030\\1.bmp",0);
	//Rect detect_rect = Rect(600, 600, 320, 300);
	//////提取检查区域的图像
	//g_srcImage2 = g_srcImage1(detect_rect);

	//Mat src;
	//g_srcImage2.copyTo(src);
	//g_dstImage = AtsoThreshold(g_srcImage1);
	//imshow("src", src);
	//测试AtsoThreshold*/
}

//shi-tomas 特征点检测
void TestShiTomas()
{
	//shi-tomas 特征点检测
	/*g_srcImage1 = imread("D:\\WRX\\X-ray\\1030\\7.bmp", 0);

	//namedWindow("dst", WINDOW_AUTOSIZE);
	//g_dAlphaValue = 1;
	//g_dBetaValue = 21;

	//g_dDeltValue = 1;
	//g_dGamaValue = 15;

	//createTrackbar("阈值1", "dst", &g_dAlphaValue, 100, on_TrackTomasTest);
	//createTrackbar("距离1", "dst", &g_dBetaValue, 100, on_TrackTomasTest);
	//////createTrackbar("阈值2", "dst", &g_dDeltValue, 100, on_TrackTomasTest);
	//////createTrackbar("距离2", "dst", &g_dGamaValue, 100, on_TrackTomasTest);
	////
	//on_TrackTomasTest(g_dAlphaValue, 0);
	//on_TrackTomasTest(g_dBetaValue, 0);
	//on_TrackTomasTest(g_dDeltValue, 0);
	//on_TrackTomasTest(g_dGamaValue, 0);
	//shi-tomas 特征点检测*/
}