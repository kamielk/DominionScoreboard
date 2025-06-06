#!bin/bash
inPath=$1
outPath=$2

for i in $(find $inPath -type f -name "*.yaml")
do
    converted=$(yq -o=json eval $i | jq '{ 
        name: .name,
        cards: .cards | map(
            . as $card
            | {
                name,
                cost: $card.cost | with_entries(select(.value > 0)),
                types: [
                    if $card.isAction then "action" else empty end,
                    if $card.isReaction == true then "reaction" else empty end,
                    if $card.isAttack then "attack" else empty end,
                    if $card.isTreasure then "treasure" else empty end,
                    if $card.isVictory then "victory" else empty end,
                    if $card.isDuration then "duration" else empty end,
                    if $card.isLiaison then "liaison" else empty end,
                    if $card.isFate then "fate" else empty end,
                    if $card.isDoom then "doom" else empty end,
                    if $card.isNight then "night" else empty end
                ],
                abilities: [
                    if $card.isTrashing then "trashing" else empty end,
                    if $card.isBuySupplier then "+buy" else empty end,
                    if $card.isActionSupplier then "+action" else empty end,
                    if $card.isMultiDrawer then "multidraw" else empty end
                ],      
            }
            | if $card.isVictory then . + { vpAlg: "?" } else . end
        )
    }')

    filename="$(basename -- $i)"
    newFilename="${filename%.*}.json"

    if [[ -n "$converted" ]]
    then
        echo $converted | jq '.' > "$outPath/$newFilename"
        echo "converted $filename to $newFilename"
    else
        echo "something went wrong while converting $filename to $newFilename"
    fi

done