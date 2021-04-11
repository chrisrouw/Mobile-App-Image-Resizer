# Mobile App Image Resizer
Utility to generate images for mobile apps

Having worked on several Xamarin apps in the past, I found that I was spending a lot of time resizing images to get the various sizes needed for Android and iOS. Inspired by some of the online icon resizers, the Mobile App Image Resizer was created. The resizing is based off the various Android screen densities (https://developer.android.com/training/multiscreen/screendensities) and iOS image sizes.

The input is an image (higher density works best), along with the desired "standard" size for the image (in pixels). The output is a zipped file with images of various sizes.
