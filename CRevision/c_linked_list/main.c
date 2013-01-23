#include<stdio.h>
#include<string.h>
#include<stdlib.h>

struct node{
	int data;
	struct node *next;
};

int main(int argk, char** argv){
	struct node header;
	header.next = NULL;
	printf("1.Add number to Queue\n2.Print List\nQ.Quit");
	char ch[2];
	do{
		printf("\n\nEnter Choice:");
		scanf("%s",ch);
		if(strcmp("1",ch)==0){
			printf("enter number:");
			int num;
			scanf("%d",&num);
			struct node* new = malloc(sizeof(struct node));
			new->data = num;
			new->next = header.next;
			header.next = new;
		}
		else if(strcmp("2",ch)==0){
			struct node* node = header.next;
			while(node!=NULL){
				printf("%d ",node->data);
				node = node->next;
			}
		}
		else{
			printf("other");
		}
	}while(strcmp("Q",ch)!=0);
	return 0;
}