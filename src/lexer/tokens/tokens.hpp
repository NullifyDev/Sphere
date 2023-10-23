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

namespace Tokens {
    enum class Type {
        _return, out, outln, colon, dollar, at,

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
}
#endif