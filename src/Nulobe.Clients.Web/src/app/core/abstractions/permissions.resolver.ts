export interface IPermissionsResolver {
    resolve: (resourceName: string, permissionName: string) => boolean;
}