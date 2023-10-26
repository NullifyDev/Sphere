//
// Created by Saturn on 23/10/2023.
//

#ifndef QBE_TOKENS_HPP
#define QBE_TOKENS_HPP

#include <iostream>
#include <fstream>
#include <sstream>
#include <optional>
#include <vector>
#include "../../../utils.hpp"

namespace Tokens {
    enum class Type {
        _return, out, outln, colon, dollar, at, exit, plus,

        numlit, strlit,

        EOL
    };

    struct Token {
        Type type;
        std::optional<std::string> value;
        int line;
        int col;
    };

    std::string ToAsm(const std::vector<Tokens::Token>& tokens);

    std::string EnumToStr(const Tokens::Token& token);
}
namespace AST {
    struct Variable {
        std::string Name;
        Tokens::Type Type;
        std::optional<std::string> Value;

        Variable(Tokens::Type type, std::string value) {
            this->Name = Utils::Gen_random(5);
            this->Type = type;
            this->Value = value;
        }
    };
}

#endif