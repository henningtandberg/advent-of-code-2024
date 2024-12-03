.text
.extern _sort
.global _solve_day1_part1

; x0 = left_numbers
; x1 = right_numbers
; x2 = line_count
_solve_day1_part1:

    ; 1. sort left_numbers and right_numbers

    sub sp, sp, #16             ; prepare space on stack
    stp x29, x30, [sp]          ; Save x29 (frame pointer) and x30 (link register)
    add x29, sp, 0              ; Set up frame pointer

    mov x4, x0                  ; save start of int *left_numbers
    mov x5, x1                  ; save start of int *right_numbers
    mov x6, x2                  ; save length

    mov x1, x2
    bl _sort                    ; sort(left_numbers, length)

    mov x0, x5
    mov x1, x6
    bl _sort                    ; sort(right_numbers, length)

    mov x0, x4                  ; restore start of int *left_numbers
    mov x1, x5                  ; restore start of int *right_numbers
    mov x2, x6                  ; restore length

    ; 2. for each number in left and right numbers
    ; 2.1.  get distance between left and right number at index i
    ; 2.2.  add distance to sum

    mov x3, #0                  ; x3 = distance and tmp
    mov x4, #0                  ; x4 = tmp
    mov x5, #0                  ; x5 = sum of distances
_L0:
    ldr w3, [x0], #4            ; x3 = *left_numbers++
    ldr w4, [x1], #4            ; x4 = *right_numbers++
    subs w3, w3, w4             ; x3 = x3 - x4
    bge end_abs                 ; if x3 > 0 skip negation
    neg w3, w3                  ; x3 = ~x3
end_abs:
    add w5, w5, w3              ; x5 += x3
    subs x2, x2, #1             ; decrement line_count by 1
    bne _L0                     ; jump to _L0 if greater than 0

    ; 3. return sum
    mov x0, #0                  ; make sure there is no garbage
    mov w0, w5

    ldp x29, x30, [sp]          ; Restore x29 and x30
    add sp, sp, #16             ; Restore stack pointer

    ret
