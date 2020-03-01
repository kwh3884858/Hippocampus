#include "CharacterUtility.h"

int CharacterUtility::BUFFERSIZE = 265;

void CharacterUtility::reverse(char * c)
{
    int length =static_cast<int>(strlen(c)) ;
    int count = length / 2;
    char temp;
    for (int i = 0; i < count; i++)
    {
        temp = c[i];
        c[i] = c[length - 1 - i];
        c[length - 1 - i] = temp;
    }
}

void CharacterUtility::parseSuffix(const char * c, char * suffix)
{
    readSuffix(c, suffix);
    reverse(suffix);
}

void CharacterUtility::wreverse(wchar_t * c)
{
    int length =static_cast<int>( wcslen(c) );
    int count = length / 2;
    wchar_t temp;
    for (int i = 0; i < count; i++)
    {
        temp = c[i];
        c[i] = c[length - 1 - i];
        c[length - 1 - i] = temp;
    }
}

void CharacterUtility::wparseSuffix(const wchar_t * c, wchar_t * suffix)
{
    wreadSuffix(c, suffix);
    wreverse(suffix);
}

size_t CharacterUtility::convertWcsToMbs(char * pDest,const wchar_t * pSrc, int BUFFERSIZE)
{
    size_t size;

#ifdef _WIN32
    wcstombs_s(&size, pDest, (size_t)BUFFERSIZE, pSrc, (size_t)BUFFERSIZE);
#else
    size = wcstombs(pDest, pSrc, BUFFERSIZE);
#endif

    return size;
}

size_t CharacterUtility::convertMbsToWcs(wchar_t * pDest, const char * pSrc, int BUFFERSIZE)
{
    size_t size;

#ifdef _WIN32
    mbstowcs_s(&size, pDest, (size_t)BUFFERSIZE, pSrc, (size_t)BUFFERSIZE);
#else
    size = mbstowcs(pDest, pSrc, BUFFERSIZE);
#endif

    return size;
}

int CharacterUtility::Find(const char* pContent, size_t contentLegth, const char* pFind, size_t findLength){
    DFA* dfa = new DFA();
    CreateDFA(pFind, findLength, dfa);
    int j =0;
    for (int i =0; i < contentLegth ; i++) {
        j = dfa->Get(dfa->GetCharacterPos(pContent[i]), j);

        if (j == findLength) {
            delete dfa;
            return i + 1 - findLength;
        }
    }
    delete dfa;
    return -1;
}


int CharacterUtility::FindLast(const char* pContent, size_t cotentLength, const char* pFind, size_t findLength){
    DFA dfa ;
    CreateReverseDFA(pFind, findLength, &dfa);
    int currentPos =0;
    for (int i = cotentLength - 1; i >= 0; i--) {
        currentPos = dfa.Get(dfa.GetCharacterPos( pContent[i] ), currentPos);

        if (currentPos == -1) {
            return i + 1;
        }
    }

    return -1;
}

bool CharacterUtility::copyCharPointer(char * pDest, const char * pSrc)
{
    if (pDest == nullptr)
    {
        return false;
    }
    int len = strlen(pSrc) + 1;

#ifdef _WIN32
    strcpy_s(pDest, len, pSrc);
#else
    strncpy(pDest, pSrc, len);
#endif

    return true;
}

bool CharacterUtility::copyWcharPointer(wchar_t * pDest, const wchar_t * pSrc)
{
    if (pDest == nullptr)
    {
        return false;
    }
    int len = wcslen(pSrc) + 1;
    
#ifdef _WIN32
    wcscpy_s(pDest, len, pSrc);
#else
    wcsncpy(pDest, pSrc, len);
#endif
    return true;
}

#ifndef _WIN32
<<<<<<< HEAD
 char* CharacterUtility::itoa(int num,char* str, int radix){
    /* 索引表 */
=======
char* CharacterUtility::itoa(int num,char* str, int radix){
    /*索引表*/
>>>>>>> d00e2c7ed079c976de3b1e67bacc961f48b27cbd
    char index[]="0123456789ABCDEF";
    unsigned unum;/* 中间变量 */
    int i=0,j,k;
    /* 确定unum的值 */
    if(radix==10&&num<0)/* 十进制负数 */
    {
        unum=(unsigned)-num;
        str[i++]='-';
    }
    else unum=(unsigned)num;/* 其他情况 */
    /* 转换 */
    do{
        str[i++]=index[unum%(unsigned)radix];
        unum/=radix;
    }while(unum);
    str[i]='\0';
    /* 逆序 */
    if(str[0]=='-')
        k=1;/* 十进制负数 */
    else
        k=0;

    for(j=k;j<=(i-1)/2;j++)
    {       char temp;
        temp=str[j];
        str[j]=str[i-1+k-j];
        str[i-1+k-j]=temp;
    }
    return str;
}
#endif

void CharacterUtility::readSuffix(const char * c, char * suffix)
{
    int i = 0, j = 0;
    int length = strlen(c);
    for (i = length - 1; i >= 0; i--)
    {
        if (c[i] == '.')
        {
            suffix[j] = '\0';
            break;
        }
        suffix[j] = c[i];

        j++;
    }
}

void CharacterUtility::wreadSuffix(const wchar_t * c, wchar_t * suffix)
{
    int i = 0, j = 0;
    int length = wcslen(c);
    for (i = length - 1; i >= 0; i--)
    {
        if (c[i] == '.')
        {
            suffix[j] = '\0';
            break;
        }
        suffix[j] = c[i];

        j++;
    }
}

CharacterUtility::DFA::DFA(){
    m_next = nullptr;
    m_character = nullptr;
    m_characterCount= 0;
    m_countentlength = 0;
}

CharacterUtility::DFA::~DFA(){
    if (m_next) {
        delete m_next;
        m_next = nullptr;
    }

    if (m_character) {
        delete m_character;
        m_character = nullptr;
    }
}



int CharacterUtility::DFA::Get(int characterPos, int pos){
    if (characterPos < 0) {
        return 0;
    }
    return m_next[characterPos * m_countentlength + pos];
}


void CharacterUtility::DFA::Set(int characterPos, int pos, int value){
    m_next[characterPos * m_countentlength + pos] = value;
}

char CharacterUtility::DFA::GetCharacter(int pos){
    return m_character[pos];
}
int CharacterUtility::DFA::GetCharacterPos(char character){
    for (int i = 0 ; i < m_characterCount; i++) {
        if (m_character[i] == character){
            return i;
        }
    }
    return -1;
}

void CharacterUtility::CreateDFA(const char* findContent,int findLength, DFA* pOutDFA ){
    int character[265];
    int count  = 0;

    //Count character occurrences
    memset(character, 0, sizeof(character));
    for (int i = 0 ; i < findLength ; i++) {
        character[findContent[i]]++;
    }
    for (int i = 0; i < 265; i++) {
        if (character[i] != 0) {
            count++;
        }
    }

    pOutDFA->m_characterCount = count;
    pOutDFA->m_countentlength = findLength;

    pOutDFA->m_next = new int[pOutDFA->m_characterCount * pOutDFA->m_countentlength];
    memset(pOutDFA->m_next, 0, pOutDFA->m_characterCount * pOutDFA->m_countentlength * 4);
    pOutDFA->m_character = new char[pOutDFA->m_characterCount];

    count = 0;
    for (int i = 0; i < 265; i++) {
        if (character[i] != 0) {
            pOutDFA->m_character[count++] = i ;
        }
    }

    int X = 0;
    pOutDFA->Set(pOutDFA->GetCharacterPos( findContent[0] ), 0, 1) ;
    for (int i = 1; i < pOutDFA->m_countentlength; i++) {
        for (int j = 0; j < pOutDFA->m_characterCount; j++) {
            pOutDFA->Set(j, i, pOutDFA->Get(j, X));
        }
        int charPos = pOutDFA->GetCharacterPos( findContent[i] );

        pOutDFA->Set(charPos, i, i+1);

        X = pOutDFA->Get(charPos, X);
    }

}

void CharacterUtility::CreateReverseDFA(const char* findContent,size_t findLength, DFA* pOutDFA ){

    int character[265];
int charCount;
    memset(character, 0, sizeof(character));

    for (int i = 0; i < findLength; i++) {
        character[ findContent[i] ]++;
    }


    for (int i = 0; i < 265; i++) {
        if (character[i] > 0) {
            charCount++;
        }
    }
    pOutDFA->m_countentlength = findLength;
    pOutDFA->m_characterCount = charCount;

    pOutDFA->m_next = new int[pOutDFA->m_countentlength * pOutDFA->m_characterCount ];
    memset(pOutDFA->m_next, 0, pOutDFA->m_characterCount * pOutDFA->m_countentlength * 4);
    pOutDFA->m_character = new char[ pOutDFA->m_characterCount];

    charCount = 0;
    for (int i = 0; i < 265; i++) {
        if (character[i] > 0) {
            pOutDFA->m_character[charCount++] = i;
        }
    }

    // Initialize last row
    int lastContentPos = 0;

    int lastCharPos = pOutDFA->GetCharacterPos(findContent[findLength - 1]);
    for (int i = 0; i < pOutDFA->m_characterCount; i++) {
        pOutDFA->Set(lastCharPos, i,  pOutDFA->m_countentlength  - 1 );
    }

    pOutDFA->Set(lastCharPos, pOutDFA->m_countentlength -1, pOutDFA->m_countentlength  - 2);

    for (int i = pOutDFA->m_countentlength - 2 ; i >= 0; i--) {
        for (int j = 0; pOutDFA->m_characterCount; j++) {
            pOutDFA->Set(j, i, pOutDFA->Get(j, lastContentPos));
        }
        // Set next one
        char characterPos = pOutDFA->GetCharacterPos( findContent[i] );
        pOutDFA->Set(characterPos, i, i - 1);

        //Update Last content row
        lastContentPos = pOutDFA->Get(characterPos, lastContentPos);

    }
}

CharacterUtility::CharacterUtility()
{

}


CharacterUtility::~CharacterUtility()
{
}
