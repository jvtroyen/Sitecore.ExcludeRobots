# Exclude Robots #

## What ##

An improved mechanism to filter robot visits to our Sitecore-sites.

## Why ##

Sitecore, out of the box, does not provide a way of having wildcards in the name of the useragent.
Since there are many crawlers/spiders/bots/whatever out there that frequently (if not constantly) change their useragent-string (with adapted version numbers), this change became needed.

## Compatibility ##

The module was created and tested on Sitecore 8.1 update-3.

## Usage ##

### Installation ###

The module is made available on the Sitecore marketplace as a Sitecore package. The package includes:

- a config file that includes a pipeline processor
- a sample file "Sitecore.Analytics.ExcludeRobots.config" that replaces the default one
- the dll
 
## History ##
- v1.0 : initial release
