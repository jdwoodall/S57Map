Notes on S57Map.  There seems to be very little information about how to actually display S-57 maps using OSGeo and SharpMap. This is my first attempt at it. So far, I can decode the S-57 data and push into a tree so that you can see what each layer contains. I have not started on the S-52 rendering section. The display function does not work at this time in any meaningful way.

I work on this intermittently, so don't expect huge progress.

ToDo: 

1. Filter the layers to actually display. I doubt anyone would want to display them all. There needs to be an option panel to control which layers to display added.  Which leads to #2.

2. Get some configuration control system working.  Most likely will use the .net config stuff.

3. Get a primitive S-52 rendering solution working. 

4. Get a tiling scheme working, most likely using BruTile. 

5. Get graphic icons for the various S-57 features. My plan, currently, is to use SVG for this.
