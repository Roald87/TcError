# TcError

Functions and datatypes which describe TwinCAT errors.

## Example

Can be used to wrap existing TwinCAT Function Blocks to add clear error codes and messages.

![example.png]

### Code

```
FUNCTION_BLOCK FB_CoeReaderEx EXTENDS FB_EcCoESdoReadEx
VAR_OUTPUT
    eAdsError : TcError.AdsErrorCodes;
END_VAR
VAR
    errorDescription : Tc2_System.T_MaxString;
END_VAR

SUPER^();
eAdsError := TcError.ToAdsErrorCode(SUPER^.nErrId);
errorDescription := TcError.AdsErrorCodeDescription(eAdsError);
```

## Manual

`TYPE AdsErrorCodes` : Enum containing all ADS error codes.
`FUNCTION ToAdsErrorCode`: Convert an ADS error code of type `UDINT` to the `AdsErrorCodes` datatype.
`FUNCTION AdsErrorCodeDescription`: Returns an description of the error from the `AdsErrorCodes` datatype.

### Example

```
PROGRAM MAIN
VAR
	adsErrorId : UDINT := 1802;
	adsErrorCode : AdsErrorCodes;
	errorDescription : T_MaxString;
END_VAR	

adsErrorCode := TcError.ToAdsErrorCode(adsErrorId); // ADSERR_DEVICE_NOMEMORY;
errorDescription := TcError.AdsErrorCodeDescription(adsErrorCode); // 'Insufficient memory.'
```
