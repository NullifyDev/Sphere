#include "utils.hpp"

bool Eof(std::string content, int index) {
    return index >= content.length();
}

void Out(std::basic_string<char> msg) {
    std::cout << msg << std::endl;
}

void Errout(std::basic_string<char> msg) {
    std::cerr << msg << std::endl;
}

std::string Gen_random(const int len) {
    static const char alphanum[] =
            "0123456789"
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            "abcdefghijklmnopqrstuvwxyz";
    std::string tmp_s;
    tmp_s.reserve(len);

    for (int i = 0; i < len; ++i) {
        tmp_s += alphanum[rand() % (sizeof(alphanum) - 1)];
    }

    return tmp_s;
}

template <typename T>
bool ExistsIn(std::vector<T> vec, T item) {
    if (std::find(vec.begin(), vec.end(), item) != vec.end())
        return true;
}