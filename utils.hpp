//
// Created by Saturn on 23/10/2023.
//

#ifndef QBE_UTILS_HPP
#define QBE_UTILS_HPP

#include <iostream>
#include <string>
#include <vector>

namespace Utils {
    bool Eof(std::string content, int index);
    void Out(std::basic_string<char> msg);
    void Errout(std::basic_string<char> msg);
    std::string Gen_random(const int len);
    template<typename T>
    bool ExistsIn(std::vector<T> vec, T item);
}
#endif //QBE_UTILS_HPP
