import { NulobeWebPage } from './app.po';

describe('nulobe App', () => {
  let page: NulobeWebPage;

  beforeEach(() => {
    page = new NulobeWebPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!');
  });
});
