import argparse
from idlelib.help import copy_strip
from itertools import pairwise
from sys import orig_argv


def is_dip(prev, curr, nexxt):
    return prev < curr and curr > nexxt or prev > curr and curr < nexxt

def is_bad_level(prev, curr, nexxt):
    if curr == nexxt or prev == curr:
        return True
    
    if is_dip(prev, curr, nexxt):
        return True

    prev_curr_is_safe = 0 < abs(prev - curr) < 4
    curr_nexxt_is_safe = 0 < abs(curr - nexxt) < 4
    
    if not curr_nexxt_is_safe or not prev_curr_is_safe:
        return True
    
    return False

def check_if_safe_with_single_bad_level(numbers):

    copy_a = []
    copy_b = []
    copy_c = []

    for i in range (1, len(numbers) - 1):
        prev = numbers[i - 1]
        curr = numbers[i]
        nexxt = numbers[i + 1]
        
        # print(f'{prev} {curr} {nexxt}')
        if is_bad_level(prev, curr, nexxt):
            copy_a = numbers[:]
            copy_a.pop(i - 1)
            copy_b = numbers[:]
            copy_b.pop(i)
            copy_c = numbers[:]
            copy_c.pop(i + 1)
            break
    
    return check_if_safe(copy_a) or check_if_safe(copy_b) or check_if_safe(copy_c)

def check_if_safe(numbers, tolerate_single_bad_level = False):
    number_pairs = list(pairwise(numbers))
    differences = [abs(x - y) for (x, y) in number_pairs]
    all_are_decreasing = all([x > y for (x, y) in number_pairs])
    all_are_increasing = all([x < y for (x, y) in number_pairs])
    
    is_safe = False
    if all_are_decreasing or all_are_increasing:
        is_safe = all([0 < difference < 4 for difference in differences])
    
    if not is_safe and tolerate_single_bad_level:
        is_safe = check_if_safe_with_single_bad_level(numbers)

    return is_safe

def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--puzzleFilePath', type = str, help = 'Path til puzzle file')
    args = parser.parse_args()
    
    debug_print = True
    
    with open(args.puzzleFilePath) as puzzleFile:
         lines = [line.rstrip() for line in puzzleFile]
     
    if debug_print:
        for line in lines:
            print(line)
    
    list_of_numbers = [list(map(lambda x: int(x), line.split(' '))) for line in lines]
    
    if debug_print:
        for numbers in list_of_numbers:
            print(numbers)
    
    checks = []
    amount_of_safe_reports = 0
    for numbers in list_of_numbers:
        is_safe = check_if_safe(numbers, True)
        if debug_print:
            print(f'{numbers} = {is_safe}')
        if is_safe:
            amount_of_safe_reports = amount_of_safe_reports + 1
    
    if debug_print:
        print(checks)
        
    print(f'Amount of safe reports: {amount_of_safe_reports}')

if __name__ == '__main__':
    main()
