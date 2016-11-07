# FastImageEditor

Basically a light-weight Image Processor App... for ~~Windows Forms~~ (the goal is to not be tied to a particular framework)

This aims to be a high-performance Imaging app put to daily use for image-related tasks.
Written in C# and linked against custom-tuned C++ libraries.

The project has 3 components
- C++ Core with algorithms implementation
- C# Test Sandbox App
- C# Image manipulation program with support for vector shapes.

# Note
This is mostly a research project.
For start it supports SIMD for x86-64 only. The plans are to add support for CUDA and/or PowerPC. Of interest is the relative comparison of some algorithms on these platforms.

# Features:

- Multicore aware, tries to split workloads where possible (OpenMP).
- maximize the potential of your desktop x86/x86-64 Intel or Amd CPU by using SIMD data paralellism. 
- Dynamic CPU Dispatching - it runs the code most optimal for the detected CPU (at runtime).
- Pure "C" interface, allowing it to be used from higher level frameworks, e.g in this case a NET app.

# Preliminary Benchmarks

- A few algorithms I've tested on large data sets (up to 8K). 
What can be seen from this is the significant single-thread advantage of an Intel Ivy Bridge CPU compared to my AMD Kaveri desktop.

![alt tag](https://raw.github.com/theradeonxt/FastImageEditor/master/Capture.PNG)[alt tag]

- Note: The composition algorithms (AlphaBlend32bgra) have been used to speed up drawing of large images in C# WindowsForms. This makes it possible to use the System.Drawing.Graphics classes. Their performance is on par (if not better) with the native BitBlt from WinAPI.
