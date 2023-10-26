#include <iostream>
#include <string>

#include "tokens.hpp"
#include "../../../utils.hpp"

std::string EnumToStr(const Tokens::Type& type) {
    switch(type) {
        case Tokens::Type::_return:
            return "return";
        case Tokens::Type::out:
            return "out";
        case Tokens::Type::outln :
            return "outln";
        case Tokens::Type::colon :
            return "colon";
        case Tokens::Type::dollar :
            return "dollar";
        case Tokens::Type::at :
            return "at";
        case Tokens::Type::exit :
            return "exit";
        case Tokens::Type::plus :
            return "plus";

        case Tokens::Type::numlit :
            return "numlit";
        case Tokens::Type::strlit :
            return "strlit";

        case Tokens::Type::EOL :
            return "EOL";
    }
    return "null";
}

std::string Tokens::ToAsm(const std::vector<Tokens::Token>& tokens) {
    std::vector<Tokens::Type> implementedfuncs;
    std::stringstream start, output, data_section, text_section, _function;

    output <<       "global _start\n";

    data_section << "section .data\n";

    text_section << "section .text\n";
    start <<        "_start:\n";




    std::vector<std::string> functions;

    for (int i = 0; i < tokens.size(); i++) {
        const Tokens::Token& token = tokens.at(i);
        switch (token.type) {
            case Tokens::Type::out:
            case Tokens::Type::outln:

                if (i + 1 < tokens.size()) {
                    int j = 1;
                    std::stringstream str;
                    while (i + j < tokens.size() && tokens.at(i + j).type != Tokens::Type::EOL) {

                        // after the print instruction, if one of the tokens of the instruction argument is a plus type, remove last space.
                        if (tokens.at(i).type == Tokens::Type::plus && tokens.at(i - 1).value.value() == " ") {

                            std::string outputString = output.str();
                            outputString = outputString.substr(0, outputString.length() - 1);

                            output.str(outputString);
                        } else {
                            str << tokens.at(i+j).value.value() << " ";
                        }

                        j++;
                    }
                    AST::Variable output = AST::Variable(Tokens::Type::strlit, str.str());
                    token.type == Tokens::Type::outln ?
                        data_section << "    " << output.Name << " db, \"" << output.Value.value() << "\", 0xA, 0x00\n":
                        data_section << "    " << output.Name << " db, \"" << output.Value.value() << "\", 0x00\n";

                    data_section << "    " << output.Name <<"len equ $ - " << output.Name;

                    start << "    mov rcx, msg\n";
                    start << "    mov rdx, " << output.Name << "\n";
                    start << "    syscall\n\n";

                    i += j - 1;
                }

                continue;
            case Tokens::Type::exit:
                if (!Utils::ExistsIn(implementedfuncs, token.type)) {
                    _function << "exit:\n";
                    _function << "    push rdi\n";
                    _function << "    mov rax, 60\n";
                    _function << "    syscall\n";
                    _function << "    ret";
                }
                i + 1 < tokens.size() ?
                    start << "    mov rdi, " << tokens.at(i+1).value.value():
                    start << "    mov rdi, 0";
                start << "    call exit";
                continue;
        }
    }


    output << data_section.str();
    output << "\n";
    output << start.str();
    output << "\n\n";

    return output.str();
}