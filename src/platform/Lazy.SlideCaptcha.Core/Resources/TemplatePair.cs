using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.SlideCaptcha.Core.Resources
{
    public class TemplatePair
    {
        public TemplatePair() { }

        public TemplatePair(Resource slider, Resource hole)
        {
            Slider = slider;
            Hole = hole;
        }

        public Resource Slider { get; set; }
        public Resource Hole { get; set; }

        public static TemplatePair Create(Resource slider, Resource hole)
        { 
            return new TemplatePair(slider, hole);
        }
    }
}
