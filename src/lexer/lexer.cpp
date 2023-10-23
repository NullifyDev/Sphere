//
// Created by Saturn on 23/10/2023.
//
#include <iostream>
#include <string>
#include <vector>

#include "tokens/tokens.hpp"
#include "lexer.hpp"
#include "../../utils.hpp"

std::vector<Tokens::Token> Tokenize(const std::string str) {
    out("[Tokenizing...]");
    std::vector<Tokens::Token> tokens;
    std::string buff;

    int col = 1, line = 1;

    for (int i = 0; i < str.length(); i++) {
        char c = str.at(i);
        if (std::isalpha(c)) {
            buff.push_back(c);
            i++;
            while (std::isalnum(str.at(i))) {
                buff.push_back(str.at(i));
                i++;
            }
            i--;
        }
        if (buff == "outln") {
            tokens.push_back({.type = Tokens::Type::outln});
            buff.clear();
            continue;
        }else if (buff == "ret") {
            tokens.push_back({.type = Tokens::Type::_return});
            buff.clear();
            continue;
        }
        else if (c == '\"') {
            i++;
            while(str.at(i) != '\"') {
                buff.push_back(str.at(i));
                i++;
            }
            tokens.push_back({.type = Tokens::Type::strlit, .value = buff});
            buff.clear();
            continue;
        } else if (std::isdigit(c)) {
            while(!eof(str, i) && std::isdigit(str.at(i))) {
                buff.push_back(str.at(i));
                i++;
            }
            i--;
            tokens.push_back({.type = Tokens::Type::numlit, .value = buff});
            buff.clear();
            continue;
        } else if (c == '\n') {
            tokens.push_back({.type = Tokens::Type::EOL});
            line++;
            col = 1;
            continue;
        } else if (std::isspace(c)) {
            continue;
        } else {
            errout("Unknown token: " + buff);
            exit(EXIT_FAILURE);
        }
    }
    return tokens;
}