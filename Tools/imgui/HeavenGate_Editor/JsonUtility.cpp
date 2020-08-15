#include "JsonUtility.h"

namespace HeavenGateEditor {



void GetCharPointerException(char*const des, const json & j, const char* const index)
{
    try
    {
        strcpy(des, j.at(index).get_ptr<const json::string_t *>()->c_str());
    }
    catch (json::exception& e)
    {
        printf("\n --!-- Get Content Exception\n message: %s \n exception id: %d \n lack of: %s \n\n", e.what(), e.id, index);
        memset(des, '\0', sizeof(des));
    }
}

void GetJsonException(json & des, const json& src, const char* const index)
{
    try
    {
        des = src.at(index);
    }
    catch (json::exception& e)
    {
        printf("\n --!-- Get Json Exception\n message: %s \n exception id: %d \n lack of: %s \n\n", e.what(), e.id, index);
        des = json();
    }
}
}
