#include<stdio.h>
#include<ctype.h>

int main(int argk, char** argv)
{
	int a,b;
	a = atoi(argv[1]);
	b = atoi(argv[2]);
	printf("The result is %d\n",a+b);
	return 0;
}