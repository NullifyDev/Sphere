#include "utils.hpp"

bool eof(std::string content, int index) {
    return index >= content.length();
}

void out(std::basic_string<char> msg) {
    std::cout << msg << std::endl;
}

void errout(std::basic_string<char> msg) {
    std::cerr << msg << std::endl;
}