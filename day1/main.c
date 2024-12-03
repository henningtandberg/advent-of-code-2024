/**
 * Methods needed:
 * 1. char to int
 * 2. sort
 * 3. max or min
 * 4. distance between two points
 *  - Distance = largest - smallest
 * 5. sum list
 */

#define DEBUG_PRINT
#undef DEBUG_PRINT

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <errno.h>
#include "day1.h"

int count_lines(FILE *puzzle_file)
{
    int count = 0;
    char c;

    // Make sure we are at the start of the file
    fseek(puzzle_file, 0, SEEK_SET);

    for (c = getc(puzzle_file); c != EOF; c = getc(puzzle_file))
        if (c == '\n')
            count++;

    // Reset file to beginning
    fseek(puzzle_file, 0, SEEK_SET);
    return count;
}

void create_numbers(FILE *puzzle_file, int *left_numbers, int *right_numbers)
{
    const int buffer_size = 256;
    char str[buffer_size];
    int index = 0;

    while(fgets(str, buffer_size, puzzle_file)) {
        char *token = strtok(str, " ");
        sscanf(token, "%05d", &left_numbers[index]);
        token = strtok(NULL, " ");
        sscanf(token, "%05d", &right_numbers[index]);
        index++;
    }
}

int main(int argc, char **argv)
{
    if (argc < 2)
    {
        fprintf(stderr, "Usage: $> ./main <PUZZLE_FILE>.txt\n");
        return -1;
    }

    char *puzzle_file_path = argv[1];
    FILE *puzzle_file = fopen(puzzle_file_path, "r");

    if (puzzle_file == NULL)
    {
        fprintf(stderr, "Failed to open %s for the following reason", puzzle_file_path, strerror(errno));
        return errno;
    }

    int line_count = count_lines(puzzle_file);
    size_t array_size = sizeof(int) * line_count;

#ifdef DEBUG_PRINT
    printf("Line count: %d\n", line_count);
#endif

    int *left_numbers = (int *)malloc(array_size);
    int *right_numbers = (int *)malloc(array_size);

    memset((void *)left_numbers, 0, array_size);
    memset((void *)right_numbers, 0, array_size);

    create_numbers(puzzle_file, left_numbers, right_numbers);

#ifdef DEBUG_PRINT
    for (int i = 0; i < line_count; i++)
        printf("%d - %d\n", left_numbers[i], right_numbers[i]);
#endif

    int solution_day1_part1 = solve_day1_part1(left_numbers, right_numbers, line_count);
    printf("Solution Day 1, Part 1: %d\n", solution_day1_part1);

    int solution_day1_part2 = solve_day1_part2(left_numbers, right_numbers, line_count);
    printf("Solution Day 1, Part 2: %d\n", solution_day1_part2);

#ifdef DEBUG_PRINT
    printf("After sort\n");
    for (int i = 0; i < line_count; i++)
        printf("%d - %d\n", left_numbers[i], right_numbers[i]);
#endif

    fclose(puzzle_file);

    free(left_numbers);
    free(right_numbers);

    return 0;
}