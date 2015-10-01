using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingMapImagery
{
    public enum ImagerySet
    {
        Aerial = 0, //– Aerial imagery.

        AerialWithLabels = 1, //–Aerial imagery with a road overlay.

        Road = 2,//– Roads without additional imagery.

        OrdnanceSurvey = 3,//--Ordnance Survey imagery. This imagery is visible only for the London area.

        CollinsBart = 4//– Collins Bart imagery. This imagery is visible only for the London area.
    }
}
