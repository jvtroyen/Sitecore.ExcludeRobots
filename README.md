# Exclude Robots #

## What ##

An improved mechanism to filter robot vitis to our Site.

## Why ##

Sitecore, out of the box, does not provide a way of having wildcards in the name of the useragent.
Since there are many crawlers/spiders/bots/whatebr out there that frequently (if not constantly) change their useragent-string (with adapted version numbers), this change became needed.

## Compatibility ##

The module was created and tested on Sitecore 8.1 update-3.

## Usage ##

### Installation ###

The module is made available on the Sitecore marketplace as a Sitecore package. The package includes:

- a config file that includes a pipeline processor
- the dll
 
## History ##
- v1.0 : initial release
