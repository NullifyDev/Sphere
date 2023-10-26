cmake_minimum_required(VERSION 3.26)
project(Qbe)

set(CMAKE_CXX_STANDARD 20)

add_executable(Qbe main.cpp
        utils.cpp
        utils.hpp


        src/lexer/tokens/tokens.cpp
        src/lexer/tokens/tokens.hpp

        src/lexer/lexer.cpp
        src/lexer/lexer.hpp
        src/Parser/parser.cpp
)
