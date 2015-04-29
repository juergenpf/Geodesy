This is a C# class library containing some very basic geodesic algorithms.
It is based on work by Mike Gavaghan (see http://www.gavaghan.org/blog/free-source-code/geodesy-library-vincentys-formula/)
and has been enhanced by us to cover also some variants of Mercators projection of the earth to
flat maps. We cover Spherical and Elliptical Mercator projections, mapping the earth to a single
flat map. We also handle the Universal Transverse Mercator (UTM) projection, dividing the earth into
smaller grids which are then each mapped to a flat map.
Finally - based on UTM - we implement an algorithm to put a finer grain mesh over the mapped area of
the earth to be able to classify a geo-location by a discrete globally unique mesh number. This
is done in order to facilitacte the application of some discrete algorithms - especially in the
area of machine learning - on geo locations.
   