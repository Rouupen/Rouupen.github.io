class UBaseState;

USTRUCT(BlueprintType)
struct FAvailableStates
{
public:
    GENERATED_BODY()

    UPROPERTY(EditAnywhere)
    TArray<TSubclassOf<UBaseState>> AvailableStates;
};


UCLASS()
class DUSTY_API UStateActionsDataAsset : public UDataAsset
{
public:
    GENERATED_BODY()

    UPROPERTY(EditAnywhere)
    TMap<TSubclassOf<UBaseState>, FAvailableStates> StatePriorityMap;
};