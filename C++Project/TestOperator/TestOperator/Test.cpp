#include "stdafx.h"
#include "Test.h"

#include <iostream>

#include "a.h"
using namespace std;

CTest::CTest()
{
}


CTest::~CTest()
{
}

void CTest::Test(int c)
{
	std::cout << "hello!" << endl;

	a b;
	b.b();
	b.c();
}