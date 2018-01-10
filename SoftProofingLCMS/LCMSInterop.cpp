/////////////////////////////////////////////////////////////////////////////////
//
// Soft Proofing Effect Plugin for Paint.NET
//
// This software is provided under the MIT License:
//   Copyright (C) 2016-2018 Nicholas Hayes
//
// See LICENSE.txt for complete licensing and attribution information.
//
/////////////////////////////////////////////////////////////////////////////////

#include "lcms2.h"
#include "LCMSInterop.h"
#include <memory.h>
#include "UnicodeIOHandler.h"

cmsHPROFILE __stdcall OpenColorProfileFromFile(const wchar_t* fileName)
{
	cmsIOHANDLER* io = OpenIOHandlerFromUnicodeFile(nullptr, fileName, UnicodeIOHandlerRead);
	if (io == nullptr)
	{
		return nullptr;
	}

	return cmsOpenProfileFromIOhandlerTHR(nullptr, io);
}

cmsBool __stdcall SaveColorProfileToFile(cmsHPROFILE hProfile, const wchar_t* fileName)
{
	cmsIOHANDLER* io = OpenIOHandlerFromUnicodeFile(nullptr, fileName, UnicodeIOHandlerWrite);
	if (io == nullptr)
	{
		return FALSE;
	}

	cmsBool result = cmsSaveProfileToIOhandler(hProfile, io) != 0;
	result &= cmsCloseIOhandler(io);

	return result;
}

cmsBool __stdcall CloseColorProfile(cmsHPROFILE hProfile)
{
	return cmsCloseProfile(hProfile);
}

cmsUInt32Number __stdcall GetColorProfileInfoSize(cmsHPROFILE hProfile, cmsInfoType info)
{
	return cmsGetProfileInfo(hProfile, info, "en", "US", nullptr, 0U);
}

cmsUInt32Number __stdcall GetColorProfileInfo(cmsHPROFILE hProfile, cmsInfoType info, wchar_t* buffer, cmsUInt32Number bufferSize)
{
	return cmsGetProfileInfo(hProfile, info, "en", "US", buffer, bufferSize);
}

ProfileColorSpace __stdcall GetProfileColorSpace(cmsHPROFILE hProfile)
{
	ProfileColorSpace colorSpace;
	int lcmsColorSpace = _cmsLCMScolorSpace(cmsGetColorSpace(hProfile));

	switch (lcmsColorSpace)
	{
	case PT_XYZ:
		colorSpace = ColorSpaceXYZ;
		break;
	case PT_Lab:
	case PT_LabV2:
		colorSpace = ColorSpaceLab;
		break;
	case PT_YUV:
		colorSpace = ColorSpaceLuv;
		break;
	case PT_YCbCr:
		colorSpace = ColorSpaceYCbCr;
		break;
	case PT_Yxy:
		colorSpace = ColorSpaceYxy;
		break;
	case PT_RGB:
		colorSpace = ColorSpaceRGB;
		break;
	case PT_GRAY:
		colorSpace = ColorSpaceGray;
		break;
	case PT_HSV:
		colorSpace = ColorSpaceHSV;
		break;
	case PT_HLS:
		colorSpace = ColorSpaceHLS;
		break;
	case PT_CMYK:
		colorSpace = ColorSpaceCMYK;
		break;
	case PT_CMY:
		colorSpace = ColorSpaceCMY;
		break;
	case PT_MCH2:
		colorSpace = ColorSpaceMultiChannel2;
		break;
	case PT_MCH3:
		colorSpace = ColorSpaceMultiChannel3;
		break;
	case PT_MCH4:
		colorSpace = ColorSpaceMultiChannel4;
		break;
	case PT_MCH5:
		colorSpace = ColorSpaceMultiChannel5;
		break;
	case PT_MCH6:
		colorSpace = ColorSpaceMultiChannel6;
		break;
	case PT_MCH7:
		colorSpace = ColorSpaceMultiChannel7;
		break;
	case PT_MCH8:
		colorSpace = ColorSpaceMultiChannel8;
		break;
	case PT_MCH9:
		colorSpace = ColorSpaceMultiChannel9;
		break;
	case PT_MCH10:
		colorSpace = ColorSpaceMultiChannel10;
		break;
	case PT_MCH11:
		colorSpace = ColorSpaceMultiChannel11;
		break;
	case PT_MCH12:
		colorSpace = ColorSpaceMultiChannel12;
		break;
	case PT_MCH13:
		colorSpace = ColorSpaceMultiChannel13;
		break;
	case PT_MCH14:
		colorSpace = ColorSpaceMultiChannel14;
		break;
	case PT_MCH15:
		colorSpace = ColorSpaceMultiChannel15;
		break;
	default:
		colorSpace = ColorSpaceUnknown;
		break;
	}

	return colorSpace;
}

cmsUInt32Number __stdcall GetProfileRenderingIntent(cmsHPROFILE hProfile)
{
	return cmsGetHeaderRenderingIntent(hProfile);
}

void __stdcall SetProfileRenderingIntent(cmsHPROFILE hProfile, cmsUInt32Number renderingIntent)
{
	cmsSetHeaderRenderingIntent(hProfile, renderingIntent);
}

void __stdcall SetGamutWarningColor(const cmsUInt8Number red, const cmsUInt8Number green, const cmsUInt8Number blue)
{
	cmsUInt16Number alarmCodes[cmsMAXCHANNELS];
	memset(alarmCodes, 0, sizeof(alarmCodes));
	alarmCodes[0] = red << 8;
	alarmCodes[1] = green << 8;
	alarmCodes[2] = blue << 8;

	cmsSetAlarmCodes(alarmCodes);
}

cmsHTRANSFORM __stdcall CreateProofingTransformBGRA8(
	cmsHPROFILE inputProfile,
	cmsHPROFILE displayProfile,
	cmsUInt32Number displayIntent,
	cmsHPROFILE proofingProfile,
	cmsUInt32Number proofingIntent,
	cmsUInt32Number flags
	)
{
	return cmsCreateProofingTransform(inputProfile, TYPE_BGRA_8, displayProfile, TYPE_BGRA_8, proofingProfile, proofingIntent, displayIntent, flags);
}

static int GetPixelFormatBytesPerPixel(const LCMSInteropPixelFormats format)
{
	switch (format)
	{
	case GRAY8:
		return 1;
	case BGR8:
		return 3;
	case CMYK8:
	case BGRA8:
	default:
		return 4;
	}
}

void __stdcall ApplyProofingTransform(
	cmsHTRANSFORM transform,
	const LCMSInteropBitmapData* source,
	LCMSInteropBitmapData* dest,
	const GdiPlusRectangle* rois,
	const int length
	)
{
	const int srcBytesPerPixel = GetPixelFormatBytesPerPixel(source->format);
	const int dstBytesPerPixel = GetPixelFormatBytesPerPixel(dest->format);

	for (int i = 0; i < length; i++)
	{
		const GdiPlusRectangle rect = rois[i];
		const int top = rect.Y;
		const int bottom = top + rect.Height;

		const int srcColumnOffset = rect.X * srcBytesPerPixel;
		const int dstColumnOffset = rect.X * dstBytesPerPixel;

		for (int y = top; y < bottom; y++)
		{
			const cmsUInt8Number* srcRow = reinterpret_cast<const cmsUInt8Number*>(source->scan0) + (y * source->stride) + srcColumnOffset;
			cmsUInt8Number* dstRow = reinterpret_cast<cmsUInt8Number*>(dest->scan0) + (y * dest->stride) + dstColumnOffset;

			cmsDoTransform(transform, srcRow, dstRow, static_cast<cmsUInt32Number>(rect.Width));
		}
	}
}

void __stdcall DeleteTransform(cmsHTRANSFORM hTransform)
{
	cmsDeleteTransform(hTransform);
}

static cmsUInt32Number PixelFormatToLCMSFormat(const LCMSInteropPixelFormats format)
{
	switch (format)
	{
	case BGR8:
		return TYPE_BGR_8;
	case GRAY8:
		return TYPE_GRAY_8;
	case CMYK8:
		return TYPE_CMYK_8;
	case BGRA8:
	default:
		return TYPE_BGRA_8;
	}
}

ConvertProfileStatus __stdcall ConvertToProfile(
	cmsHPROFILE inputProfile,
	cmsHPROFILE outputProfile,
	cmsUInt32Number renderingIntent,
	cmsUInt32Number transformFlags,
	const LCMSInteropBitmapData* input,
	LCMSInteropBitmapData* output
	)
{
	if (inputProfile == nullptr || outputProfile == nullptr || input == nullptr || output == nullptr)
	{
		return ConvertProfileStatusInvalidParameter;
	}

	if (input->width != output->width || input->height != output->height)
	{
		return ConvertProfileStatusDifferentImageDimensions;
	}

	cmsHTRANSFORM transform = cmsCreateTransform(
		inputProfile,
		PixelFormatToLCMSFormat(input->format),
		outputProfile,
		PixelFormatToLCMSFormat(output->format),
		renderingIntent,
		transformFlags
		);

	if (transform == nullptr)
	{
		return ConvertProfileStatusCreateTransformFailed;
	}

	for (cmsUInt32Number y = 0; y < input->height; y++)
	{
		const cmsUInt8Number* srcRow = reinterpret_cast<const cmsUInt8Number*>(input->scan0) + (y * input->stride);
		cmsUInt8Number* dstRow = reinterpret_cast<cmsUInt8Number*>(output->scan0) + (y * output->stride);

		cmsDoTransform(transform, srcRow, dstRow, input->width);
	}

	cmsDeleteTransform(transform);

	return ConvertProfileStatusOk;
}
