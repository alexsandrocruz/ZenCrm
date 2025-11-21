import { registerLicense } from '@syncfusion/ej2-base';

/**
 * Syncfusion License Configuration
 *
 * This file handles the registration of the Syncfusion license key.
 * The license key removes the watermark from Syncfusion components.
 *
 * License Key: Ngo9BigBOggjHTQxAR8/V1JFaF5cXGRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWH5ccXRcQ2ddV0NwVkFWYEw=
 */

export const SYNCUSION_LICENSE_KEY = 'Ngo9BigBOggjHTQxAR8/V1JFaF5cXGRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWH5ccXRcQ2ddV0NwVkFWYEw=';

/**
 * Register the Syncfusion license key
 * Call this function once during application startup
 */
export function registerSyncfusionLicense(): void {
  try {
    registerLicense(SYNCUSION_LICENSE_KEY);
    console.log('Syncfusion license registered successfully');
  } catch (error) {
    console.error('Failed to register Syncfusion license:', error);
  }
}

/**
 * Syncfusion license validation
 * This can be used to verify if the license is valid (optional)
 */
export function validateSyncfusionLicense(): boolean {
  try {
    // The license is registered if no error is thrown
    registerLicense(SYNCUSION_LICENSE_KEY);
    return true;
  } catch (error) {
    console.error('Syncfusion license validation failed:', error);
    return false;
  }
}