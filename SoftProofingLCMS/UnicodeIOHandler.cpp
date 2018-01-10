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

#include "lcms2_plugin.h"
#include "UnicodeIOHandler.h"
#include <cstdio>
#include <io.h>

static cmsUInt32Number FileRead(cmsIOHANDLER* ioHandler, void* buffer, cmsUInt32Number size, cmsUInt32Number count)
{
	size_t elementsRead = fread(buffer, size, count, reinterpret_cast<FILE*>(ioHandler->stream));

	if (elementsRead != count)
	{
		cmsSignalError(ioHandler->ContextID, cmsERROR_FILE, "Read Error, read %d bytes, block should be %d bytes", size * elementsRead, size * count);
		return 0;
	}

	return count;
}

static cmsBool FileSeek(cmsIOHANDLER* ioHandler, cmsUInt32Number offset)
{
	int error = fseek(reinterpret_cast<FILE*>(ioHandler->stream), static_cast<long>(offset), SEEK_SET);

	if (error != 0)
	{
		cmsSignalError(ioHandler->ContextID, cmsERROR_FILE, "Seek Error, fseek returned error code: %d", errno);
		return FALSE;
	}

	return TRUE;
}

static cmsUInt32Number FileTell(cmsIOHANDLER* ioHandler)
{
	long pos = ftell(reinterpret_cast<FILE*>(ioHandler->stream));

	if (pos == -1L)
	{
		cmsSignalError(ioHandler->ContextID, cmsERROR_FILE, "Tell Error, ftell returned error code: %d", errno);
		return 0;
	}

	return static_cast<cmsUInt32Number>(pos);
}

static cmsBool FileWrite(cmsIOHANDLER* ioHandler, cmsUInt32Number size, const void* buffer)
{
	if (size == 0)
	{
		return TRUE;
	}

	ioHandler->UsedSpace += size;

	const size_t elementCount = 1;

	return (fwrite(buffer, size, elementCount, reinterpret_cast<FILE*>(ioHandler->stream)) == elementCount);
}

static cmsBool FileClose(cmsIOHANDLER* ioHandler)
{
	cmsBool result = fclose(reinterpret_cast<FILE*>(ioHandler->stream)) == 0;
	_cmsFree(ioHandler->ContextID, ioHandler);

	return result;
}

cmsIOHANDLER* OpenIOHandlerFromUnicodeFile(const cmsContext contextID, const wchar_t* fileName, const UnicodeIOHandlerAccess access)
{
	cmsIOHANDLER* ioHandler = nullptr;
	FILE* file = nullptr;
	errno_t error;
	long fileLength;

	ioHandler = reinterpret_cast<cmsIOHANDLER*>(_cmsMallocZero(contextID, sizeof(cmsIOHANDLER)));
	if (ioHandler == nullptr)
	{
		return nullptr;
	}

	switch (access)
	{
	case UnicodeIOHandlerRead:
		error = _wfopen_s(&file, fileName, L"rb");
		if (error != 0)
		{
			_cmsFree(contextID, ioHandler);
			cmsSignalError(contextID, cmsERROR_FILE, "Read error, _wfopen_s returned error code: %d", error);
			return nullptr;
		}

		fileLength = _filelength(_fileno(file));
		if (fileLength == -1)
		{
			fclose(file);
			_cmsFree(contextID, ioHandler);
			cmsSignalError(contextID, cmsERROR_FILE, "Unable to get the size of the file, invalid file descriptor?");
			return nullptr;
		}

		ioHandler->ReportedSize = static_cast<cmsUInt32Number>(fileLength);
		break;

	case UnicodeIOHandlerWrite:
		error = _wfopen_s(&file, fileName, L"wb");
		if (error != 0)
		{
			_cmsFree(contextID, ioHandler);
			cmsSignalError(contextID, cmsERROR_FILE, "Write error, _wfopen_s returned error code: %d", error);
			return nullptr;
		}
		ioHandler->ReportedSize = 0;
		break;

	default:
		_cmsFree(contextID, ioHandler);
		cmsSignalError(contextID, cmsERROR_FILE, "Unknown UnicodeIOHandlerAccess value '%d'", access);
		return nullptr;
	}

	ioHandler->ContextID = contextID;
	ioHandler->stream = static_cast<void*>(file);
	ioHandler->UsedSpace = 0;
	// Set the PhysicalFile to NUL because according to MSDN converting wide character file names
	// to ANSI may produce incorrect results when used with some system code pages.
	// https://msdn.microsoft.com/en-us/library/windows/desktop/dd374047(v=vs.85).aspx#SC_char_sets_in_file_names
	ioHandler->PhysicalFile[0] = 0;

	ioHandler->Read = FileRead;
	ioHandler->Seek = FileSeek;
	ioHandler->Tell = FileTell;
	ioHandler->Write = FileWrite;
	ioHandler->Close = FileClose;

	return ioHandler;
}