This is a C# class library containing some very basic geodesic algorithms.
It is based on work by Mike Gavaghan (see http://www.gavaghan.org/blog/free-source-code/geodesy-library-vincentys-formula/)
and has been enhanced by me to cover also some variants of Mercators projection of the earth to
flat maps. I cover Spherical and Elliptical Mercator projections, mapping the earth to a single
flat map. I also handle the Universal Transverse Mercator (UTM) projection, dividing the earth into
smaller grids which are then each mapped to a flat map.
Finally - based on UTM - I implement an algorithm to put a finer grain mesh over the mapped area of
the earth to be able to classify a geo-location by a discrete globally unique mesh number. This
is done in order to facilitacte the application of some discrete algorithms - especially in the
area of machine learning - on geo locations.

You may access this library in your own projects through my personal NuGet feed. The feed URL
is: https://home.familiepfeifer.de/nuFeed/nuget
You must add that feed in VisualStudio (Tools -> Options -> NuGet Package Manager -> Package Sources)

  