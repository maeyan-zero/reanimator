# Introduction #

Add your content here.


# Medpack #

Index: 75

## Props1 ##
| **Alias** | **Size** | **Notes** |
|:----------|:---------|:----------|
| Token     | Uint32   | Const: 26 |
| Value     | Uint32   | Heal value |
| Null      | Uint32   | null      |

## **##
The following four are constant in format throughout the items.txt.cooked file.**

## fixedLevel ##
| **Alias** | **Size** | **Notes** |
|:----------|:---------|:----------|
| Token     | Uint32   | Const.26  |
| Value     | Uint32   | Level Value |
| Null      | Uint32   | null      |

## minBaseDmg ##
| **Alias** | **Size** | **Notes** |
|:----------|:---------|:----------|
| Token     | Uint32   | Const.26  |
| Value     | Uint32   | Min Base Damage Value |
| Null      | Uint32   | null      |

## maxBaseDmg ##
| **Alias** | **Size** | **Notes** |
|:----------|:---------|:----------|
| Token     | Uint32   | Const.26  |
| Value     | Uint32   | Max Base Damage Value |
| Null      | Uint32   | null      |

## stackSize ##
| **Alias** | **Size** | **Notes** |
|:----------|:---------|:----------|
| Token     | Uint32   | Const.26  |
| Value     | Uint32   | Stack Value |
| Null      | Uint32   | null      |

## **##
The next four each have 2 variations in format.
One is a fixed value, the other is a formula of some form.
I doubt that the formulae all have constants, but they do all seem to be the same for each. There doesn't appear to be anything that defines the formula(like operators) in the cooked files.**

## sellPriceMult small ##
| **Alias** | **Size** | **Notes** |
|:----------|:---------|:----------|
| Token     | Uint32   | Const.26  |
| Value     | Uint32   | Sell Price Value |
| Null      | Uint32   | null      |

## sellPriceMult large ##
| **Alias** | **Size** | **Notes** |
|:----------|:---------|:----------|
| Token     | Uint32   | Const.26  |
| Value     | Uint32   | Const.100 |
| Value     | Uint32   | Const.708 |
| Value     | Uint32   | Const.26  |
| Value     | Uint32   | Const.250 |
| Value     | Uint32   | Const.03  |
| Value     | Uint32   | Const.98  |
| Value     | Uint32   | Const.26  |
| Value     | Uint32   | Const.01  |
| Value     | Uint32   | Const.399 |
| Value     | Uint32   | Const.26  |
| Value     | Uint32   | Const.15  |
| Value     | Uint32   | Const.358 |
| Value     | Uint32   | Const.392 |
| Null      | Uint32   | null      |

## buyPriceMult small ##
| **Alias** | **Size** | **Notes** |
|:----------|:---------|:----------|
| Token     | Uint32   | Const.26  |
| Value     | Uint32   | Buy Price Value |
| Null      | Uint32   | null      |

## buyPriceMult large ##
| **Alias** | **Size** | **Notes** |
|:----------|:---------|:----------|
| Token     | Uint32   | Const.26  |
| Value     | Uint32   | Const.100 |
| Value     | Uint32   | Const.708 |
| Value     | Uint32   | Const.26  |
| Value     | Uint32   | Const.250 |
| Value     | Uint32   | Const.03  |
| Value     | Uint32   | Const.98  |
| Value     | Uint32   | Const.26  |
| Value     | Uint32   | Const.01  |
| Value     | Uint32   | Const.399 |
| Value     | Uint32   | Const.26  |
| Value     | Uint32   | Const.15  |
| Value     | Uint32   | Const.358 |
| Value     | Uint32   | Const.392 |
| Null      | Uint32   | null      |

## sellPriceAdd small ##
| **Alias** | **Size** | **Notes** |
|:----------|:---------|:----------|
| Token     | Uint32   | Const.26  |
| Value     | Uint32   | Sell Price Value |
| Null      | Uint32   | null      |

This seems to appear only on unitType 317.
## sellPriceAdd large ##
| **Alias** | **Size** | **Notes** |
|:----------|:---------|:----------|
| Token     | Uint32   | Const.26  |
| Value     | Uint32   | Const.100 |
| Value     | Uint32   | Const.708 |
| Value     | Uint32   | Const.26  |
| Value     | Uint32   | Const.250 |
| Value     | Uint32   | Const.03  |
| Value     | Uint32   | Const.98  |
| Value     | Uint32   | Const.26  |
| Value     | Uint32   | Const.199 |
| Value     | Uint32   | Const.358 |
| Value     | Uint32   | Const.392 |
| Value     | Uint32   | Const.711 |
| Value     | Uint32   | Const.03  |
| Value     | Uint32   | Const.131 |
| Value     | Uint32   | Const.03  |
| Value     | Uint32   | Const.31  |
| Null      | Uint32   | null      |

## buyPriceAdd small ##
| **Alias** | **Size** | **Notes** |
|:----------|:---------|:----------|
| Token     | Uint32   | Const.26  |
| Value     | Uint32   | Buy Price Value |
| Null      | Uint32   | null      |

This seems to appear only on unitType 317. And not all of them at that.
## buyPriceAdd large ##
| **Alias** | **Size** | **Notes** |
|:----------|:---------|:----------|
| Token     | Uint32   | Const.26  |
| Value     | Uint32   | Const.100 |
| Value     | Uint32   | Const.708 |
| Value     | Uint32   | Const.26  |
| Value     | Uint32   | Const.250 |
| Value     | Uint32   | Const.03  |
| Value     | Uint32   | Const.98  |
| Value     | Uint32   | Const.26  |
| Value     | Uint32   | Const.199 |
| Value     | Uint32   | Const.358 |
| Value     | Uint32   | Const.392 |
| Value     | Uint32   | Const.711 |
| Value     | Uint32   | Const.03  |
| Value     | Uint32   | Const.131 |
| Value     | Uint32   | Const.03  |
| Value     | Uint32   | Const.31  |
| Null      | Uint32   | null      |