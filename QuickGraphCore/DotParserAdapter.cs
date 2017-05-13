using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickGraph
{
    using Attributes = IDictionary<string, string>;

    public class DotParserAdapter
    {
        public class Common
        {
            public static int? GetWeightNullable(Attributes attrs)
            {
                int weight;
                return int.TryParse(attrs["weight"], out weight) ? (int?) weight : null;
            }

            public static int GetWeight(Attributes attrs, int defaultValue)
            {
                if (!attrs.ContainsKey("weight")) return defaultValue;

                int weight;
                return int.TryParse(attrs["weight"], out weight) ? weight : defaultValue;
            }
        }

        public class VertexFactory
        {
            public static Func<string, Attributes, string>
                Name = (v, attrs) => v;


            public static Func<string, Attributes, KeyValuePair<string, Attributes>>
                NameAndAttributes = (v, attrs) =>
                    new KeyValuePair<string, Attributes>(v, new Dictionary<string, string>(attrs));


            public static Func<string, Attributes, KeyValuePair<string, int?>>
                WeightedNullable = (v, attrs) =>
                    new KeyValuePair<string, int?>(v, DotParserAdapter.Common.GetWeightNullable(attrs));


            public static Func<string, Attributes, KeyValuePair<string, int>>
                Weighted(int defaultValue)
            {
                return (v, attrs) =>
                    new KeyValuePair<string, int>(v, DotParserAdapter.Common.GetWeight(attrs, defaultValue));
            }
        }


        public class EdgeFactory<TVertex>
        {
            public static Func<TVertex, TVertex, Attributes, SEdge<TVertex>>
                VerticesOnly = (v1, v2, attrs) => new SEdge<TVertex>(v1, v2);


            public static Func<TVertex, TVertex, Attributes, STaggedEdge<TVertex, Attributes>>
                VerticesAndEdgeAttributes = (v1, v2, attrs) =>
                    new STaggedEdge<TVertex, Attributes>(v1, v2, new Dictionary<string, string>(attrs));


            public static Func<TVertex, TVertex, Attributes, STaggedEdge<TVertex, int?>>
                WeightedNullable = (v1, v2, attrs) =>
                    new STaggedEdge<TVertex, int?>(v1, v2, DotParserAdapter.Common.GetWeightNullable(attrs));


            public static Func<TVertex, TVertex, Attributes, STaggedEdge<TVertex, int>>
                Weighted(int defaultValue)
            {
                return (v1, v2, attrs) =>
                    new STaggedEdge<TVertex, int>(v1, v2, DotParserAdapter.Common.GetWeight(attrs, defaultValue));
            }
        }
    }
}