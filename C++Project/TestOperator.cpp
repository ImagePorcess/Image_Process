// TestOperator.cpp : �������̨Ӧ�ó������ڵ㡣
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
void on_Trackbar(int, void*)//��Ӧ�������Ļص�����
{
	//double Value1 = (double) g_dAlphaValue / 100; //��ǰAlpha��������ֵ��ռ������global��double��
	//double Value2 = (1.0 - Value1); //��ǰBeta��������ֵ��ռ������global��double��
	//addWeighted(g_srcImage1, Value1, g_srcImage2, Value2, 0, g_dstImage);//ͼ��ĵ���
	//imshow("srcImage", g_dstImage);//��ָ��������ʾͼ��
	//saturate_cast�Ƿ�ֹ��������� >= 255 =255   <=0 = 0;
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
 
void on_Trackbar1(int, void*)//��Ӧ�������Ļص�����
{
	////g_dAlphaValue = (double)g_dAlphaValue / 100; //��ǰAlpha��������ֵ��ռ������global��double��
	////g_dBetaValue = (1.0 - g_dAlphaValue); //��ǰBeta��������ֵ��ռ������global��double��
	////addWeighted(g_srcImage1, g_dAlphaValue, g_srcImage2, g_dBetaValue, 0, g_dstImage);//ͼ��ĵ���
	////imshow("srcImage", g_dstImage);//��ָ��������ʾͼ��

	
}

void on_Trackbar2(int, void*) // gamma �Ļص�����
{
	//�������ұ�
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
		//  ͨ�����ұ����ת��
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

	//ORB ��Ѱ������
	//Mat srcPic = imread("D:\\WRX\\X-ray\\1030\\1.bmp");
	//Mat dstPic = srcPic.clone();

	//imshow("src", srcPic);
	//auto orb_detector = ORB::create(1000);

	//vector<KeyPoint> kpts;

	//// ����ORB���
	//orb_detector->detect(srcPic, kpts);

	//// ���ƹؼ���
	//drawKeypoints(srcPic, kpts, dstPic, Scalar(0, 0, 255), DrawMatchesFlags::DEFAULT);
	//imshow("dst", dstPic);
	//ORB ��Ѱ������
	 
	//�ǵ���
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


	//Laplacian��ͨ�˲�����
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


	//Unshrpen Mask�㷨
	/*Mat input = imread("D:\\WRX\\X-ray\\1030-1\\16-1.bmp");
	Mat blur, usm;
	GaussianBlur(input, blur, Size(0, 0), 25);
	addWeighted(input, 1.5, blur, -0.5, 0, usm);
	imshow("usm", usm);*/
	//Unshrpen Mask�㷨
	
	//canny������ȡֱ�ߺ�����
	/*Mat input = imread("D:\\WRX\\X-ray\\1030\\1.bmp");
	Mat output;
	Canny(input, output, 350, 400);
	imshow("dst", output);*/
	//canny������ȡֱ�ߺ�����

	//�����϶���
	//g_srcImage1 = imread("D:\\WRX\\X-ray\\1030\\1.bmp");
	//g_srcImage2 = imread("D:\\WRX\\X-ray\\1030-1\\16-1.bmp");
	//namedWindow("srcImage");
	//g_dAlphaValue = 10;
	//char TrackbarName[50];//���������������ƴ洢����
	//sprintf_s(TrackbarName, "͸����%d", 100);
	//createTrackbar("TrackbarName", "srcImage", &g_dAlphaValue, 100, on_Trackbar);
	//createTrackbar("TrackbarName1", "srcImage", &g_dAlphaValue, 100, on_Trackbar);

	//on_Trackbar(g_dAlphaValue, 0);//����ڻص���������ʾ
	//�����϶���

	//ʹ���϶����������ȺͶԱȶ�
	/*g_srcImage1 = imread("D:\\WRX\\X-ray\\1030\\1.bmp");
	g_dstImage = Mat::zeros(g_srcImage1.size(), g_srcImage1.type());
	g_dAlphaValue = 80;
	g_dBetaValue = 80;

	namedWindow("dst", WINDOW_AUTOSIZE);

	createTrackbar("�Աȶ�", "dst", &g_dAlphaValue, 300, on_Trackbar);
	createTrackbar("����", "dst", &g_dBetaValue, 200, on_Trackbar);

	on_Trackbar(g_dAlphaValue, 0);
	on_Trackbar(g_dBetaValue, 0);*/
	//ʹ���϶����������ȺͶԱȶ�


	//gamma�任
	g_dAlphaValue = 15;
	g_srcImage1 = imread("D:\\WRX\\X-ray\\1030\\1.bmp");
	namedWindow("dst", WINDOW_AUTOSIZE);
	createTrackbar("gamma", "dst", &g_dAlphaValue, 100, on_Trackbar2);
	on_Trackbar2(g_dAlphaValue, 0);
	//gamma�任


	//��˹�������DOG
	//Mat srcPic = imread("D:\\WRX\\X-ray\\1030\\1.bmp");
	//Mat dstPic, g1, g2, dogImg;
	//cvtColor(srcPic, dstPic, CV_RGB2GRAY);
	////���θ�˹ģ��
	//GaussianBlur(srcPic, g1, Size(3, 3), 0, 0);
	//GaussianBlur(g1, g2, Size(3, 3), 0);
	//subtract(g1, g2, dogImg, Mat());   //���ͼ�ĻҶ�ֵ�Ƚ�С��ͼ�Ƚϰ���

	////��һ����ʾ
	//normalize(dogImg, dogImg, 255, 0, NORM_MINMAX);  //��һ�����ŵ�0-255��ʾ��
	//imshow("DOG_img", dogImg);
	////DOG����
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
	//		if ((int)norm_dst.at<float>(j, i) < 140 && (int)norm_dst.at<float>(j, i) > 130)//��������ֵ�����Ľǵ㴦��Բ��

	//		{
	//			circle(normScaleDst, Point(i, j), 5, Scalar(0, 0, 255), 2, 8);
	//		}
	//	}
	//}
	//imshow("dst", normScaleDst);
	//��˹�������DOG+�ǵ����ȡ������


	//�ж�ͼƬΪ����/��ͼ
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

	
	//�ж�ͼƬΪ����/��ͼ

	//ѡ���������
	//g_srcImage1 = imread("D:\\WRX\\X-ray\\1030\\1.bmp");
	//g_srcImage2 = g_srcImage1(Rect(0, 0, 300, 300)); // ǳ����
	//Mat img1;
	//g_srcImage2.copyTo(img1);//���
	//namedWindow("src",1);
	//namedWindow("dst1",1);
	//namedWindow("dst2",1);
	//circle(g_srcImage2, Point(10, 10), 5, Scalar(0, 0, 255), 2, 8);

	//imshow("dst1", g_srcImage2);

	//imshow("dst2", img1);

	//imshow("src", g_srcImage1);
	//ѡ���������ʱ,ʹ��ǳ�������Խ��ͼ��ͼ��ߴ�,ͬʱ�Ѽ�����ݱ��浽ԭͼ��

	//uint8 inMinValue, inMaxValue;
	
	//auto cvArray = Mat(g_srcImage1);
	//auto transposedCvArray = cv::Mat(cvArray.cols, cvArray.rows, CV_8SC1);
	//transpose(cvArray, transposedCvArray);
	//NdArray<uint8> inArray = NdArray<uint8>(transposedCvArray.data, transposedCvArray.rows, transposedCvArray.cols);
	//clip(&inArray, inMinValue, inMaxValue);
	waitKey(0);
	return 0;
}