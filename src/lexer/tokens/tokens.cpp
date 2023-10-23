//
// Created by Saturn on 23/10/2023.
//

#include <iostream>
#include <string>

#include "tokens.hpp"

std::string ToAsm(const std::vector<Tokens::Token>& tokens) {
    std::stringstream output;
    output << "global _start\n_start:\n";
    for (int i = 0; i < tokens.size(); i++) {
        const Tokens::Token& token = tokens.at(i);
        switch (token.type) {
            case Tokens::Type::_return:
                if (i + 1 < tokens.size() && tokens.at(i+1).type == Tokens::Type::numlit) {
                    output << "    mov rax, 60\n";
                    output << "    mov rdi, " << tokens.at(i+1).value.value() + "\n";
                    output << "    syscall";
                }
                break;
            case Tokens::Type::outln:
                break;
            case Tokens::Type::EOL:
                continue;
            default:
                break;
        }
    }
    return output.str();
}