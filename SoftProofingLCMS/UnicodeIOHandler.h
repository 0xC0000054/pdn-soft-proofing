/////////////////////////////////////////////////////////////////////////////////
//
// Soft Proofing Effect Plugin for Paint.NET
//
// This software is provided under the MIT License:
//   Copyright (C) 2017-2018 Nicholas Hayes
//
// See LICENSE.txt for complete licensing and attribution information.
//
/////////////////////////////////////////////////////////////////////////////////

#ifndef UNICODEIOHANDLER_H
#define UNICODEIOHANDLER_H

enum UnicodeIOHandlerAccess
{
	UnicodeIOHandlerRead = 0,
	UnicodeIOHandlerWrite
};

cmsIOHANDLER* OpenIOHandlerFromUnicodeFile(const cmsContext contextID, const wchar_t* fileName, const UnicodeIOHandlerAccess access);

#endif // !UNICODEIOHANDLER_H
