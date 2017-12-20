/////////////////////////////////////////////////////////////////////////////////
//
// Soft Proofing Effect Plugin for Paint.NET
//
// This software is provided under the MIT License:
//   Copyright (C) 2016-2017 Nicholas Hayes
//
// See LICENSE.txt for complete licensing and attribution information.
//
/////////////////////////////////////////////////////////////////////////////////

#ifndef LCMSINTEROP_H
#define LCMSINTEROP_H

#ifdef __cplusplus
extern "C" {
#endif // __cplusplus


#ifdef LCMSINTEROP_EXPORTS
#define LCMSINTEROP_API __declspec(dllexport)
#else
#define LCMSINTEROP_API __declspec(dllimport)
#endif

enum ProfileColorSpace
{
	ColorSpaceUnknown = 0,
	ColorSpaceXYZ,
	ColorSpaceLab,
	ColorSpaceLuv,
	ColorSpaceYCbCr,
	ColorSpaceYxy,
	ColorSpaceRGB,
	ColorSpaceGray,
	ColorSpaceHSV,
	ColorSpaceHLS,
	ColorSpaceCMYK,
	ColorSpaceCMY,
	ColorSpaceMultiChannel2,
	ColorSpaceMultiChannel3,
	ColorSpaceMultiChannel4,
	ColorSpaceMultiChannel5,
	ColorSpaceMultiChannel6,
	ColorSpaceMultiChannel7,
	ColorSpaceMultiChannel8,
	ColorSpaceMultiChannel9,
	ColorSpaceMultiChannel10,
	ColorSpaceMultiChannel11,
	ColorSpaceMultiChannel12,
	ColorSpaceMultiChannel13,
	ColorSpaceMultiChannel14,
	ColorSpaceMultiChannel15
};

enum LCMSInteropPixelFormats
{
	// 32-bit BGRA image data, 8-bits per channel.
	BGRA8 = 0,
	// 24-bit BGR image data, 8-bits per channel.
	BGR8,
	// 8-bit gray scale image data, single channel.
	GRAY8,
	// 32-bit CMYK image data, 8-bits per channel.
	CMYK8
};

struct LCMSInteropBitmapData
{
	cmsUInt32Number width;
	cmsUInt32Number height;
	LCMSInteropPixelFormats format;
	cmsUInt32Number stride;
	void* scan0;
};

enum ConvertProfileStatus
{
	// No error
	ConvertProfileStatusOk = 0,
	// One of the method parameters is not valid.
	ConvertProfileStatusInvalidParameter = -1,
	// The input and output images are not the same size.
	ConvertProfileStatusDifferentImageDimensions = -2,
	// An error occurred when creating the color transform.
	ConvertProfileStatusCreateTransformFailed = -3
};

struct GdiPlusRectangle
{
	int X;
	int Y;
	int Width;
	int Height;
};

LCMSINTEROP_API cmsHPROFILE __stdcall OpenColorProfileFromFile(const wchar_t* fileName);

LCMSINTEROP_API cmsBool __stdcall SaveColorProfileToFile(cmsHPROFILE hProfile, const wchar_t* fileName);

LCMSINTEROP_API cmsBool __stdcall CloseColorProfile(cmsHPROFILE hProfile);

LCMSINTEROP_API cmsUInt32Number __stdcall GetColorProfileInfoSize(cmsHPROFILE hProfile, cmsInfoType info);

LCMSINTEROP_API cmsUInt32Number __stdcall GetColorProfileInfo(
	cmsHPROFILE hProfile,
	cmsInfoType info,
	wchar_t* buffer,
	cmsUInt32Number bufferSize
	);

LCMSINTEROP_API ProfileColorSpace __stdcall GetProfileColorSpace(cmsHPROFILE hProfile);

LCMSINTEROP_API cmsUInt32Number _stdcall GetProfileRenderingIntent(cmsHPROFILE hProfile);

LCMSINTEROP_API void __stdcall SetProfileRenderingIntent(cmsHPROFILE hProfile, cmsUInt32Number renderingIntent);

LCMSINTEROP_API void __stdcall SetGamutWarningColor(const cmsUInt8Number red, const cmsUInt8Number green, const cmsUInt8Number blue);

LCMSINTEROP_API cmsHTRANSFORM __stdcall CreateProofingTransformBGRA8(
	cmsHPROFILE inputProfile,
	cmsHPROFILE displayProfile,
	cmsUInt32Number displayIntent,
	cmsHPROFILE proofingProfile,
	cmsUInt32Number proofingIntent,
	cmsUInt32Number flags
	);

LCMSINTEROP_API void __stdcall ApplyProofingTransform(
	cmsHTRANSFORM transform,
	const LCMSInteropBitmapData* source,
	LCMSInteropBitmapData* dest,
	const GdiPlusRectangle* rois,
	const int length
	);

LCMSINTEROP_API void __stdcall DeleteTransform(cmsHTRANSFORM hTransform);

LCMSINTEROP_API ConvertProfileStatus __stdcall ConvertToProfile(
	cmsHPROFILE inputProfile,
	cmsHPROFILE outputProfile,
	cmsUInt32Number renderingIntent,
	cmsUInt32Number transformFlags,
	const LCMSInteropBitmapData* input,
	LCMSInteropBitmapData* output
	);

#ifdef __cplusplus
}
#endif // __cplusplus

#endif // LCMSINTEROP_H