import { VeganFactsWebPage } from './app.po';

describe('vegan-facts-web App', () => {
  let page: VeganFactsWebPage;

  beforeEach(() => {
    page = new VeganFactsWebPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!');
  });
});
