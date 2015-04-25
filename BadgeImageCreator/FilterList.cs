using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge.Imaging.Filters;

namespace BadgeImageCreator
{
	public static class FilterList
	{
		public static List<Type> GetFilterList()
		{
			return new List<Type>()
			{
				typeof(Blur),
				typeof(BrightnessCorrection),
				typeof(ChannelFiltering),
				typeof(ColorFiltering),
				typeof(ColorRemapping),
				typeof(ContrastCorrection),
				typeof(ContrastStretch),
				typeof(Edges),
				typeof(EuclideanColorFiltering),
				typeof(ExtractChannel),
				typeof(GaussianBlur),
				typeof(GaussianSharpen),
				typeof(HistogramEqualization),
				typeof(HSLFiltering),
				typeof(HSLLinear),
				typeof(HueModifier),
				typeof(Jitter),
				typeof(LevelsLinear),
				typeof(Mean),
				typeof(RotateChannels),
				typeof(SaltAndPepperNoise),
				typeof(SaturationCorrection),
				typeof(Sepia),
				typeof(Sharpen),
				typeof(SimplePosterization),
				typeof(TransformFromPolar),
				typeof(TransformToPolar),
				typeof(WaterWave),
				typeof(YCbCrFiltering),
				typeof(YCbCrLinear)
			};
		}
	}
}
