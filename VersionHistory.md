# Version History

## Pending

Add changes here when they're committed to the `master` branch. To publish, update the [version number](SolutionInfo.props), move the pending changes below to a new [Released](#released) section, and [create a new release](/FacilityApi/Facility/releases) using the version number, e.g. `v2.3.4`.

Prefix the description of each change with `[major]`, `[minor]`, or `[patch]` in accordance with [SemVer](http://semver.org).

* [minor] Support tags via attribute: `[tags(name: shiny)]`
* [minor] Support tag exclusion via command-line: `--excludeTags shiny`
* [minor] Report multiple definition errors from command-line tools.
* [minor] Improve http attribute errors.
* [major] Drop support for arbitrary HTTP methods (to help detect typos).
* [major] Upgrade to .NET Standard 2.0 and .NET 4.6.1. Upgrade NuGet dependencies.
* [major] Stop using System.Net.Http.HttpMethod.

## Released

### 1.5.0

* Start tracking version history.