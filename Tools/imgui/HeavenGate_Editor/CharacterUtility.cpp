#include "CharacterUtility.h"

size_t CharacterUtility::BUFFERSIZE = 265;

void CharacterUtility::reverse(char * c)
{
	int length = strlen(c);
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
	int length = wcslen(c);
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
	wcstombs_s(&size, pDest, (size_t)BUFFERSIZE, pSrc, (size_t)BUFFERSIZE);
	return size;
}

size_t CharacterUtility::convertMbsToWcs(wchar_t * pDest, const char * pSrc, int BUFFERSIZE)
{
	size_t size;
	mbstowcs_s(&size, pDest, (size_t)BUFFERSIZE, pSrc, (size_t)BUFFERSIZE);
	return size;
}

bool CharacterUtility::copyCharPointer(char * pDest, const char * pSrc)
{
	if (pDest != nullptr)
	{
		return false;
	}
	int len = strlen(pSrc) + 1;
	pDest = new char[len];
	strcpy_s(pDest, len, pSrc);
	
	return true;
}

bool CharacterUtility::copyWcharPointer(wchar_t * pDest, const wchar_t * pSrc)
{
	if (pDest != nullptr)
	{
		return false;
	}
	int len = wcslen(pSrc) + 1;
	pDest = new wchar_t[len];
	wcscpy_s(pDest, len, pSrc);

	return true;
}

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

CharacterUtility::CharacterUtility()
{

}


CharacterUtility::~CharacterUtility()
{
}
