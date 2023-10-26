; Credits to @megabytesofrem for assisting with this asm code

section .data
HEAP_SIZE: equ 1024 ; 1KB reserved for the heap size

section .bss
memory: resb 1024 ; 1KB
bitmap: resb 1024 ; 1KB, same size as memory

section .text
global _start

;; Check if a block of memory is free
;; Clobbers the following registers:
;; - rax: address of the bitmap
;; - rcx: loop counter
;; - dl: current byte in the bitmap
;; Outputs:
;; - r11: result of the check
%macro is_free_space 3
    ; save registers before we clobber them
    push rax
    push rcx
    push rdx

    mov rax, [%1]       ; rax stores the address of the bitmap
    mov dl, 0           ; dl stores the current byte in the bitmap
    jge .done
    jmp .check_free     ; loop through the bitmap
    .check_free:
        mov dl, [%2 + %3] ; index the bitmap by rcx
        cmp dl, 0           ; check if the current byte is 0
        jnz .next_block     ; jump to next_block if the current byte is not 0
        je .found_free      ; jump to found_free if the current byte is 0
    .next_block:
        inc rcx             ; increment the loop counter
        cmp rcx, HEAP_SIZE  ; check if the loop counter is >= HEAP_SIZE
        jge .no_free
        jmp .check_free
    .no_free:
        ; we didn't find a free block
        mov dl, 0
        jmp .done
    .found_free:
        ; we found a free block
        mov dl, 1
        jmp .done
    .done:
        mov r11, dl         ; r11 stores the result of the check

        ; restore registers
        pop rdx
        pop rcx
        pop rax
        ret
%endmacro

;; Initialize the bitmap
;; Clobbers the following registers:
;; - rax: stores the address of the bitmap
bitmap_init:
    ; save registers before we clobber them
    push rax

    ; rax stores the address of the bitmap
    mov rax, [memory]

    ; restore registers
    pop rax
    ret

bitmap_free:
    ret

;; Mark a block of memory as allocated
;; Inputs:
;; - rdi: starting address of the block
;; - rsi: size of the block
;; Clobbers the following registers:
;; - rax: address of the bitmap
;; - rcx: loop counter
;; - dl: current byte in the bitmap
mark_allocated:
    ; save registers before we clobber them
    push rdi
    push rsi
    push rax
    push rcx
    push rdx

    mov rax, [bitmap]   ; rax stores the address of the bitmap
    mov dl, 0           ; dl stores the current byte in the bitmap

    ; check if we are at the end of the bitmap
    cmp rcx, HEAP_SIZE
    jge .done
    jmp .mark_block     ; loop through the bitmap
    .mark_block:
        mov dl, [rax + rcx] ; index the bitmap by rcx
        mov byte [rax + rcx], 1
        inc rcx             ; increment the loop counter

        cmp rcx, HEAP_SIZE
        jge .done
        jmp .mark_block     ; jump back to here 
    .done:
        ; restore registers
        pop rdx
        pop rcx
        pop rax
        pop rsi
        pop rdi
        ret

;; Mark a block of memory as free
;; Inputs:
;; - rdi: starting address of the block
;; - rsi: size of the block
;; Clobbers the following registers:
;; - rax: address of the bitmap
;; - rcx: loop counter
mark_free:
    ; save registers before we clobber them
    push rdi
    push rsi
    push rax
    push rcx

    mov rax, [bitmap]   ; rax stores the address of the bitmap
    mov dl, 0           ; dl stores the current byte in the bitmap
    mov r10, rdi        ; r10 stores the starting address of the block
    mov r11, rsi        ; r11 stores the size of the block
    cmp rcx, [r10 + r11]  ; check if we are at the end of the bitmap
    jge .done
    jmp .mark_block     ; loop through the bitmap
    .mark_block:
        mov byte [rax + rcx], 0
        inc rcx             ; increment the loop counter

        cmp rcx, HEAP_SIZE  ; check if the loop counter is >= HEAP_SIZE
        jge .done
        jmp .mark_block     ; jump back to here 
    .done:
        ; restore registers
        pop rcx
        pop rax
        pop rsi
        pop rdi

        ret

;; Allocate a contiguous block of memory
;; Inputs:
;; - rdi: size of the block to allocate
;; Clobbers the following registers:
;; - rax: address of the allocated block
;; - rdi: address of the index in the bitmap
;; - rcx: loop counter
;; - dl: current byte in the bitmap
bitmap_alloc:
    cmp rdi, 0      ; check if size of block == 0
    je .zero_size   ; jump to zero_size if rdi == 0
    jnz .alloc_loop ; jump to alloc_loop if rdi != 0
    .zero_size:
        ; Cannot allocate a block of size 0
        ret
    .alloc_loop:
        ; Loop through the heap
        mov rax, [memory]
        mov rdi, [bitmap]
        mov rcx, 0
        cmp rcx, HEAP_SIZE
        jge .done
        jmp .find_free
    .find_free:
        ; Check if the current block is marked as free in the bitmap

        mov dl, [rdi]    ; dl stores the current byte in the bitmap
        cmp dl, 1        ; check if the current byte is 1
        jnz .next_block  ; jump to next_block if the current byte is not 1
        je .no_free      ; jump to no_free since we have no free blocks
    .next_block:
        inc rcx          ; increment the loop counter
        inc rdi          ; increment the bitmap index
        cmp rcx, HEAP_SIZE  ; check if the loop counter is >= HEAP_SIZE
        jge .done

        ; check if we have a free block
        is_free_space bitmap memory rcx
        cmp r11, 1
        je .found_free
        jmp .find_free
        ret
    .found_free:
        ; we found a free block, mark it as allocated
        push rsi
        push rdi
        mov rdi, rcx
        mov rsi, rdi
        jmp mark_allocated

        ; restore registers
        pop rdi
        pop rsi
        ret
    .no_free:
        ; couldn't find a free block
        ret
    .done:
        ; 

_start:
    mov rax, 60
    mov rdi, 0
    syscall
    ;ret

