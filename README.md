# EasySave

## Disclaimer
The code in this repository is currently NOT finished and could contain bugs, unfinished functionalities and generally unexpected behaviour. The project is mainly developed for research purposes and personal usage, as such it will probably not be updated frequently. Feel free to contribute or fork.

## Overview

C# .Net class library for saving data easily and securely.

Currently supports encrypted and compressed JSON-Files.

## Usage

Use <code>CreateDataobject</code> on any <code>JSONSerializable</code> to create a <code>JSONDataObject</code>

<code>JSONDataObject</code>s can be compressed and/or encrypted.

For saving use <code>JSONSerializable.Save</code>

For further information see the TestProject
