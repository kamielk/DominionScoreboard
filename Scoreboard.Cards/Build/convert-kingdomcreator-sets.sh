#!bin/bash
path=$1

for i in $(find $path -type f -name "*.yaml")
do
    converted=$(yq r -j $i | jq '{ 
        name: .name,
        cards: .cards | map({
            name,
            cost: .cost | with_entries(select(.value > 0)),
            types: [
                if .isAction then "action" else empty end,
                if .isReaction == true then "reaction" else empty end,
                if .isAttack then "attack" else empty end,
                if .isTreasure then "treasure" else empty end,
                if .isVictory then "victory" else empty end,
                if .isDuration then "duration" else empty end,
                if .isLiaison then "liaison" else empty end,
                if .isFate then "fate" else empty end,
                if .isDoom then "doom" else empty end,
                if .isNight then "night" else empty end
            ],
            abilities: [
                if .isTrashing then "trashing" else empty end,
                if .isBuySupplier then "+buy" else empty end,
                if .isActionSupplier then "+action" else empty end,
                if .isMultiDrawer then "multidraw" else empty end
            ]
        }),
    }')

    filename="$(basename -- $i)"
    newFilename="${filename%.*}.json"
    echo $converted | jq '.' > "./${newFilename}"
    echo "converted $filename to ${newFilename}"
done