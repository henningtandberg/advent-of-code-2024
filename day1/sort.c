//
// Created by Henning Tandberg on 02/12/2024.
//

#include "sort.h"

void swap(int *a, int *b) {
    int tmp = *a;
    *a = *b;
    *b = tmp;
}

// Selection Sort
extern void sort(int *v, int n)
{
    int min;

    for(int i = 0; i < n; i++){
        min = i;

        for(int j = i+1; j < n; j++)
            if(v[j] < v[min]) min = j;

        swap(&v[min], &v[i]);
    }
}