#include <iostream>
#include <fstream>
#include <sstream>
#include <vector>

#include "utils.hpp"
#include "src/lexer/tokens/tokens.hpp"
#include "src/lexer/lexer.hpp"

int main(int argc, char* argv[]) {
    if (argc != 2) {
        out("Incorrect usage. Correct usage is:");
        out("qbe <input.qbe>");
        return EXIT_FAILURE;
    }

    std::string contents;
    {
        std::stringstream contents_stream;
        std::fstream input(argv[1], std::ios::in);
        contents_stream << input.rdbuf();
        contents = contents_stream.str();
    }

    std::vector<Tokens::Token> tokens = Tokenize(contents);
    {
        std::fstream file("../out.asm", std::ios::out);
        file << Tokens::ToAsm(tokens);
    }

    return EXIT_SUCCESS;
}
